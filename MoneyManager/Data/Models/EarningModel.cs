using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MoneyManager.Shared;

namespace MoneyManager.Data.Models
{
    public class EarningModel: Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public AccountModel Account { get; set; } = new();
        public double Value { get; set; } = 0;
    }
}
