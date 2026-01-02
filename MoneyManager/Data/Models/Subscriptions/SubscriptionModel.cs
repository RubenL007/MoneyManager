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

        public DateTimeOffset? StartDate { get; set; } = null!;
        public DateTimeOffset RenewalDate { get; set; } = DateTime.Now;
        
        private DateTimeOffset? _endDate = null;
        public DateTimeOffset? EndDate
        {
            get => _endDate == DateTimeOffset.MinValue ? null : _endDate;
            set => _endDate = value;
        }

        public bool IsTerminated
        {
            get => EndDate != null && EndDate.Value < DateTime.Now;
            set
            {
                if (value)
                {
                    // Setting true when previously false: if no date, assign today
                    if (EndDate == null)
                        EndDate = DateTimeOffset.Now;
                }
            }
        }

        public int RecurringInterval { get; set; } = 1;
        public RecurringUnitEnum RecurringUnit { get; set; } = RecurringUnitEnum.Monthly;
    }
}
