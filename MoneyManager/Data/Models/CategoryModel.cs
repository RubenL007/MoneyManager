using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyManager.Data.Models
{
    public class CategoryModel : CategoryBaseModel
    {
        public List<ExpenseModel> Expenses { get; set; } = new();

        public double TotalSpent { get; set; } = 0;
    }
}
