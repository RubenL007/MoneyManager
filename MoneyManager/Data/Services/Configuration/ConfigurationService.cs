using Microsoft.Extensions.Options;
using MoneyManager.Data.Models.Configuration;
using MoneyManager.Shared.UserAuthentication;
using MoneyManager.Shared;
using MongoDB.Driver;
using IConfiguration = MoneyManager.Data.Interface.Configuration.IConfiguration;

namespace MoneyManager.Data.Services.Configuration
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

        #region ConfigurationModel GetConfiguration()
        public ConfigurationModel GetConfiguration()
        {
            string? userId = _userAuthentication.GetCurrentUserId();
            var response = _configurationsCollection.Find(a => a.UserId == userId).FirstOrDefault();
            if (response == null)
            {
                UpdateConfiguration(new());
                response = _configurationsCollection.Find(a => a.UserId == userId).FirstOrDefault();
                return response;
            }
            else
            {
                return response;
            }
        }
        #endregion
    }
}
