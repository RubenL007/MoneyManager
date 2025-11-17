
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MoneyManager.Shared;

namespace MoneyManager.Data.Models
{
    public class MonthSheetModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset Date { get; set; } = DateTime.Now;
        //Because Radzen charts don't support DateTimeOffset, this way I can bind a property
        public DateTime ChartDate => Date.DateTime;

        public List<CategoryModel> Categories { get; set; } = new();

        public List<EarningModel> Earnings { get; set; } = new();

        public double TotalSpent => Categories.Sum(c => c.TotalSpent);
        public double EstimatedSpent { get; set; } = 0;
        public double Earned => Earnings.Sum(e => e.Value);
        public double EstimatedEarned { get; set; } = 0;
        public double Balance => Earned - TotalSpent;
        public double EstimatedBalance => EstimatedEarned - EstimatedSpent;
    }
}
