using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class CategoryService : ICategory
    {
        private readonly IMongoCollection<CategoryBaseModel> _categoriesCollection;
        private readonly IUserAuthentication _userAuthentication;

        public CategoryService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _categoriesCollection = database.GetCollection<CategoryBaseModel>("Categories");

            _userAuthentication = userAuthentication;
        }

        #region CreateCategory(CategoryBaseModel category)
        public string CreateCategory(CategoryBaseModel category)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var categoryObj = _categoriesCollection.Find(c => c.UserId == userId
                                                         && c.Id == category.Id).FirstOrDefault();
            if (categoryObj == null)
            {
                category.UserId = userId!;
                _categoriesCollection.InsertOne(category);
                return "Category saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateCategory(CategoryBaseModel category)
        public string UpdateCategory(CategoryBaseModel category)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_categoriesCollection.Find(c => c.UserId == userId 
                                           && c.Id == category.Id).Any())
            {
                _categoriesCollection.ReplaceOne(c => c.UserId == userId
                                                 && c.Id == category.Id, category);
                return "Category updated sucessfully.";
            }
            else
            {
                return "The category could not be found.";
            }
        }
        #endregion

        #region CategoryBaseModel GetCategory(Guid id)
        public CategoryBaseModel GetCategory(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _categoriesCollection.Find(c => c.UserId == userId 
                                              && c.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<CategoryBaseModel> SearchCategories()
        public List<CategoryBaseModel> SearchCategories()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _categoriesCollection.Find(c => c.UserId == userId)
                                        .ToList();
        }
        #endregion

        #region DeleteCategory(Guid id)
        public string DeleteCategory(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_categoriesCollection.Find(c => c.UserId == userId 
                                           && c.Id == id).Any())
            {
                _categoriesCollection.DeleteOne(c => c.UserId == userId 
                                                && c.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The category could not be found.";
            }
        } 
        #endregion
    }
}