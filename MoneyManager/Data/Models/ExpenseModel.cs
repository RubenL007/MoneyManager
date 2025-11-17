using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MoneyManager.Shared;

namespace MoneyManager.Data.Models
{
    public class ExpenseModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public SellerModel Seller { get; set; } = new();

        public DateTimeOffset BuyDate { get; set; } = DateTimeOffset.Now;

        public AccountModel Account { get; set; } = new();

        public double Spent { get; set; } = 0;
    }
}
