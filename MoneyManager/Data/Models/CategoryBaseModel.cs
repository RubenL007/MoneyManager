using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MoneyManager.Shared;

namespace MoneyManager.Data.Models
{
    public class CategoryBaseModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
    }
}
