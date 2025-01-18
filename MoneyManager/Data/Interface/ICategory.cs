using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface ICategory
    {
        string CreateCategory(CategoryBaseModel category);
        string UpdateCategory(CategoryBaseModel category);
        CategoryBaseModel GetCategory(Guid id);
        List<CategoryBaseModel> SearchCategories();
        string DeleteCategory(Guid id);
    }
}
