using Microsoft.Extensions.Options;
using MoneyManager.Data.Models;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;
using IConfiguration = MoneyManager.Data.Interface.IConfiguration;

namespace MoneyManager.Data.Services
{
    public class ConfigurationService : IConfiguration
    {
        private readonly IMongoCollection<ConfigurationModel> _configurationsCollection;
        private readonly IUserAuthentication _userAuthentication;

        public ConfigurationService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _configurationsCollection = database.GetCollection<ConfigurationModel>("Configurations");

            _userAuthentication = userAuthentication;
        }

        #region UpdateConfiguration(ConfigurationModel configuration)
        public string UpdateConfiguration(ConfigurationModel configuration)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_configurationsCollection.Find(a => a.UserId == userId
                                         && a.Id == configuration.Id).Any())
            {
                _configurationsCollection.ReplaceOne(a => a.UserId == userId
                                               && a.Id == configuration.Id, configuration);
                return "Configuration updated sucessfully.";
            }
            else
            {
                configuration.UserId = userId!;
                _configurationsCollection.InsertOne(configuration);

                return "Configuration created with success.";
            }
        }
        #endregion

        #region List<ConfigurationModel> SearchConfiguration()
        public List<ConfigurationModel> SearchConfiguration()
        {
            string? userId = _userAuthentication.GetCurrentUserId();
            return _configurationsCollection.Find(a => a.UserId == userId).ToList();
        }
        #endregion
    }
}
