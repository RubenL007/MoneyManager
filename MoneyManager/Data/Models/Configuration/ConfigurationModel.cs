using MoneyManager.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyManager.Data.Models.Configuration
{
    public class ConfigurationModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<CategoryModel> DefaultCategories { get; set; } = new();
        public double DefaultEstimatedSpent { get; set; } = 0;
        public double DefaultEstimatedEarned { get; set; } = 0;
        public double DefaultEstimatedBalance => DefaultEstimatedEarned - DefaultEstimatedSpent;
    }
}
