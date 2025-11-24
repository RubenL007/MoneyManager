using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Data.Models.Configuration;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;
using IConfiguration = MoneyManager.Data.Interface.Configuration.IConfiguration;

namespace MoneyManager.Data.Services
{
    public class MonthSheetService : IMonthSheet
    {
        private readonly IMongoCollection<MonthSheetModel> _monthsCollection;
        private readonly IUserAuthentication _userAuthentication;
        private IConfiguration _configurationService;

        public MonthSheetService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication, IConfiguration configurationService)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _monthsCollection = database.GetCollection<MonthSheetModel>("MonthsSheets");
            _userAuthentication = userAuthentication;
            _configurationService = configurationService;
        }

        #region CreateMonthSheet(MonthSheetModel monthSheet)
        public string CreateMonthSheet(MonthSheetModel monthSheet)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            //fix DateTimeOffset error in mongo
            var start = new DateTime(monthSheet.Date.Year, monthSheet.Date.Month, 1);
            var end = start.AddMonths(1);

            var monthSheetObj = _monthsCollection.Find(m => m.UserId == userId 
                                                         && m.Id == monthSheet.Id 
                                                         || (m.UserId == userId && m.Date >= start && m.Date < end))
                                                 .FirstOrDefault();

            if (monthSheetObj == null)
            {
                ConfigurationModel configurationResponse = _configurationService.GetConfiguration();

                monthSheet.UserId = userId!;
                if (configurationResponse.DefaultCategories.Any()) monthSheet.Categories = configurationResponse.DefaultCategories;
                monthSheet.EstimatedEarned = configurationResponse.DefaultEstimatedEarned;
                monthSheet.EstimatedSpent = configurationResponse.DefaultEstimatedSpent;
                _monthsCollection.InsertOne(monthSheet);
                return "Month Sheet saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateMonthSheet(MonthSheetModel monthSheet)
        public string UpdateMonthSheet(MonthSheetModel monthSheet)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_monthsCollection.Find(m => m.UserId == userId
                                       && m.Id == monthSheet.Id).Any())
            {
                _monthsCollection.ReplaceOne(m => m.UserId == userId
                                             && m.Id == monthSheet.Id, monthSheet);
                return "Month Sheet updated successfully.";
            }
            else
            {
                return "The month sheet could not be found.";
            }
        }
        #endregion

        #region MonthSheetModel GetMonthSheet(Guid id)
        public MonthSheetModel GetMonthSheet(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _monthsCollection.Find(m => m.UserId == userId
                                          && m.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<MonthSheetModel> SearchMonthSheets()
        public List<MonthSheetModel> SearchMonthSheets()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _monthsCollection.Find(m => m.UserId == userId)
                                    .ToList();
        }
        #endregion

        #region DeleteMonthSheet(Guid id)
        public string DeleteMonthSheet(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_monthsCollection.Find(m => m.UserId == userId
                                       && m.Id == id).Any())
            {
                _monthsCollection.DeleteOne(m => m.UserId == userId
                                            && m.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The month sheet could not be found.";
            }
        } 
        #endregion
    }
}
