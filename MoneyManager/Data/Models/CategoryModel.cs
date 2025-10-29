using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MoneyManager.Shared;

namespace MoneyManager.Data.Models
{
    public class CategoryModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<ExpenseModel> Expenses { get; set; } = new();
        public double TotalSpent => Expenses.Sum(x => x.Spent);
        public string Name { get; set; } = string.Empty;
    }
}
