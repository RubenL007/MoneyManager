using MoneyManager.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyManager.Data.Models.Subscriptions
{
    public class SubscriptionModel : Tenantable
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public double Value { get; set; } = 0;
        public AccountModel PaymentAccount { get; set; } = new();
        public SellerModel Seller { get; set; } = new();

        public DateTime? StartDate { get; set; } = null!;
        public DateTime RenewalDate { get; set; } = DateTime.Now;
        
        private DateTime? _endDate = null;
        public DateTime? EndDate
        {
            get => _endDate == DateTime.MinValue ? null : _endDate;
            set => _endDate = value;
        }

        public bool IsTerminated
        {
            get => EndDate != null;
            set
            {
                if (value)
                {
                    // Setting true when previously false: if no date, assign today
                    if (EndDate == null)
                        EndDate = DateTime.Today;
                }
                else
                {
                    EndDate = null;
                }
            }
        }
        public int RecurringInterval = 1;
        public RecurringUnitEnum RecurringUnit = RecurringUnitEnum.Month;
    }
}
