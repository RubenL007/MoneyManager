using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models.Balance
{
    public class BalanceModel: Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset Date { get; set; } = DateTime.Now;

        public List<AccountModel> Accounts { get; set; } = new List<AccountModel>();

        public double TotalValue => Accounts.Sum(a => a.Value);
    }
}
