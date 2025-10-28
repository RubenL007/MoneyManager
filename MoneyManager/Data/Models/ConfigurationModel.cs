using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models
{
    public class ConfigurationModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<CategoryModel> DefaultCategories { get; set; } = new();
        public double DefaultEstimatedSpent { get; set; } = 0;
        public double DefaultEstimatedEarned { get; set; } = 0;
        public double DefaultEstimatedBalance { get; set; } = 0;  
    }
}
