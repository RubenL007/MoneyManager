using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models.Balance
{
    public class BalanceModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        //Because Radzen charts don't support DateTimeOffset, this way I can bind a property
        public DateTime ChartDate => Date.DateTime;

        public List<AccountModel> Accounts { get; set; } = new List<AccountModel>();

        public double TotalValue => Accounts.Sum(a => a.Value);
    }
}
