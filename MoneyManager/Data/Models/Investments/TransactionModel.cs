using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models.Investments
{
    public class TransactionModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string TransactionID { get; set; } = string.Empty;
        public bool IsBuyOperation { get; set; } = true;
        public DateTimeOffset TransactionDate { get; set; } = DateTime.Now;

        public double Money { get; set; } = 0;
        public double TransactionTax { get; set; } = 0;
        public double TransactionedUnits { get; set; } = 0;
        public double PricePerUnit { get; set; } = 0;
    }
}
