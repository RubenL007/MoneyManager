using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface.Subscriptions;
using MoneyManager.Data.Models.Subscriptions;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services.Subscriptions
{
    public class SubscriptionService : ISubscription
    {
        private readonly IMongoCollection<SubscriptionModel> _subscriptionsCollection;
        private readonly IUserAuthentication _userAuthentication;

        public SubscriptionService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _subscriptionsCollection = database.GetCollection<SubscriptionModel>("Subscriptions");

            _userAuthentication = userAuthentication;
        }

        #region CreateSubscription(SubscriptionModel subscription)
        public string CreateSubscription(SubscriptionModel subscription)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var subscriptionObj = _subscriptionsCollection.Find(a => a.UserId == userId
                                                                  && a.Id == subscription.Id).FirstOrDefault();
            if (subscriptionObj == null && !string.IsNullOrWhiteSpace(subscription.Name))
            {
                subscription.UserId = userId!;
                _subscriptionsCollection.InsertOne(subscription);
                return "Subscription saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateSubscription(SubscriptionModel Subscription)
        public string UpdateSubscription(SubscriptionModel subscription)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_subscriptionsCollection.Find(a => a.UserId == userId
                                               && a.Id == subscription.Id).Any())
            {
                _subscriptionsCollection.ReplaceOne(a => a.UserId == userId
                                                      && a.Id == subscription.Id, subscription);
                return "Subscription updated successfully.";
            }
            else
            {
                return "The Subscription could not be found.";
            }
        }
        #endregion

        #region SubscriptionModel GetSubscription(Guid id)
        public SubscriptionModel GetSubscription(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _subscriptionsCollection.Find(a => a.UserId == userId
                                                   && a.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<SubscriptionModel> SearchSubscriptions()
        public List<SubscriptionModel> SearchSubscriptions()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _subscriptionsCollection.Find(a => a.UserId == userId).ToList();
        }
        #endregion

        #region DeleteSubscription(Guid id)
        public string DeleteSubscription(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_subscriptionsCollection.Find(a => a.UserId == userId
                                                && a.Id == id).Any())
            {
                _subscriptionsCollection.DeleteOne(a => a.UserId == userId
                                                     && a.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The account could not be found.";
            }
        }
        #endregion
    }
}
