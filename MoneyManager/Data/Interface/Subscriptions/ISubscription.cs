using MoneyManager.Data.Models.Subscriptions;

namespace MoneyManager.Data.Interface.Subscriptions
{
    public interface ISubscription
    {
        string CreateSubscription(SubscriptionModel Subscription);
        string UpdateSubscription(SubscriptionModel Subscription);
        SubscriptionModel GetSubscription(Guid id);
        List<SubscriptionModel> SearchSubscriptions();
        string DeleteSubscription(Guid id);

    }
}
