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
        private readonly IMongoCollection<CategoryModel> _categoriesCollection;
        private readonly IUserAuthentication _userAuthentication;

        public CategoryService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _categoriesCollection = database.GetCollection<CategoryModel>("Categories");

            _userAuthentication = userAuthentication;
        }

        #region CreateCategory(CategoryModel category)
        public string CreateCategory(CategoryModel category)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var categoryObj = _categoriesCollection.Find(c => c.UserId == userId
                                                         && c.Id == category.Id).FirstOrDefault();
            if (categoryObj == null && !string.IsNullOrWhiteSpace(category.Name))
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

        #region UpdateCategory(CategoryModel category)
        public string UpdateCategory(CategoryModel category)
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

        #region CategoryModel GetCategory(Guid id)
        public CategoryModel GetCategory(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _categoriesCollection.Find(c => c.UserId == userId 
                                              && c.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<CategoryModel> SearchCategories()
        public List<CategoryModel> SearchCategories()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var cenas =  _categoriesCollection.Find(c => c.UserId == userId)
                                        .ToList();
            return cenas;
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