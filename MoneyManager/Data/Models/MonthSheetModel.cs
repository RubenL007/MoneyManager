
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyManager.Data.Models
{
    public class MonthSheetModel
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Date { get; set; } = DateTime.Now;

        public List<CategoryModel> Categories { get; set; } = new();

        public List<EarningModel> Earnings { get; set; } = new();

        public double TotalSpent { get; set; } = 0;
        public double EstimatedSpent { get; set; } = 0;
        public double Earned { get; set; } = 0;
        public double EstimatedEarned { get; set; } = 0;
        public double Balance { get; set; } = 0;
        public double EstimatedBalance { get; set; } = 0;
    }
}
