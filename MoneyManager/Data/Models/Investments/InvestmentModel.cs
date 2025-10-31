using MoneyManager.Data.Models.Investment;
using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models.Investments
{
    public class InvestmentModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public AccountModel Account { get; set; } = new();
        public InvestmentTypeEnum? InvestmentType { get; set; } = null!;
        public List<TransactionModel> Transactions { get; set; } = new();

        public double TotalInvested => Transactions.Where(t => t.IsBuyOperation).Sum(t => t.Money);
        public double TotalSold => Transactions.Where(t => !t.IsBuyOperation).Sum(t => t.Money);
        public double CurrentBalance => TotalInvested - TotalSold;

        public double TotalTaxPayed => Transactions.Sum(t => t.TransactionTax);

        public double TotalUnitsBought => Transactions.Where(t => t.IsBuyOperation).Sum(t => t.TransactionedUnits);
        public double TotalUnitsSold => Transactions.Where(t => !t.IsBuyOperation).Sum(t => t.TransactionedUnits);
        public double CurrentUnitsBalance => TotalUnitsBought - TotalUnitsSold;
        //public double AverageUnitPrice => {
        //    Transactions.Where(t => t.IsBuyOperation).Sum;
        //    ]
        //TODO
    }
}
