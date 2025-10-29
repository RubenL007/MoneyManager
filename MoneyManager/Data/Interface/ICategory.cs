using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface ICategory
    {
        string CreateCategory(CategoryModel category);
        string UpdateCategory(CategoryModel category);
        CategoryModel GetCategory(Guid id);
        List<CategoryModel> SearchCategories();
        string DeleteCategory(Guid id);
    }
}
