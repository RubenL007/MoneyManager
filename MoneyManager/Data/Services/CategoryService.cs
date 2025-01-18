using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class CategoryService : ICategory
    {
        private MongoClient _mongoClient = null!;
        private IMongoDatabase _database = null!;
        private IMongoCollection<CategoryBaseModel> _categoriesCollection = null!;
        public CategoryService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("MoneyManager");
            _categoriesCollection = _database.GetCollection<CategoryBaseModel>("Categories");
        }

        public string CreateCategory(CategoryBaseModel category)
        {
            var categoryObj = _categoriesCollection.Find(x => x.Id == category.Id).FirstOrDefault();
            if (categoryObj == null)
            {
                _categoriesCollection.InsertOne(category);
                return "Category saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }

        public string UpdateCategory(CategoryBaseModel category)
        {
            if (_categoriesCollection.Find(x => x.Id == category.Id).Any())
            {
                _categoriesCollection.ReplaceOne(x => x.Id == category.Id, category);
                return "Category updated sucessfully.";
            }
            else
            {
                return "The category could not be found.";
            }
        }

        public CategoryBaseModel GetCategory(Guid id)
        {
            return _categoriesCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<CategoryBaseModel> SearchCategories()
        {
            return _categoriesCollection.Find(FilterDefinition<CategoryBaseModel>.Empty).ToList();
        }

        public string DeleteCategory(Guid id)
        {
            if (_categoriesCollection.Find(x => x.Id == id).Any())
            {
                _categoriesCollection.DeleteOne(x => x.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The category could not be found.";
            }
        }
    }
}