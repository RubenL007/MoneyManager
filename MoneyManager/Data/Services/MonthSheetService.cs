using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Interface.Subscriptions;
using MoneyManager.Data.Models;
using MoneyManager.Data.Models.Configuration;
using MoneyManager.Data.Models.Subscriptions;
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
        private ISubscription _subscriptionService;

        public MonthSheetService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication, IConfiguration configurationService, ISubscription subscriptionService)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _monthsCollection = database.GetCollection<MonthSheetModel>("MonthsSheets");
            _userAuthentication = userAuthentication;
            _configurationService = configurationService;
            _subscriptionService = subscriptionService;
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
                if (configurationResponse.DefaultCategories.Any()) monthSheet.Categories.AddRange(configurationResponse.DefaultCategories);
                monthSheet.EstimatedEarned = configurationResponse.DefaultEstimatedEarned;
                monthSheet.EstimatedSpent = configurationResponse.DefaultEstimatedSpent;

                List<SubscriptionModel> subscriptionsResponse = _subscriptionService.SearchSubscriptions();
                if (subscriptionsResponse.Any())
                {
                    #region Add Subscriptions Expenses of the month
                    CategoryModel subscriptionsCategory = new()
                    {
                        Name = "Subscriptions",
                        UserId = subscriptionsResponse.First().UserId
                    };
                    if (!monthSheet.Categories.Any(c => c.Name == "Subscriptions"))
                    {
                        monthSheet.Categories.Add(subscriptionsCategory);
                    }
                    foreach (var sub in subscriptionsResponse)
                    {
                        #region standard subExpense to add
                        ExpenseModel subExpense = new()
                        {
                            Account = sub.PaymentAccount,
                            //BuyDate = 
                            Name = sub.Name,
                            Seller = sub.Seller,
                            Spent = sub.Value,
                            UserId = sub.UserId
                        };
                        #endregion
                        if (sub.StartDate != null && sub.StartDate.Value.Year == monthSheet.Date.Year
                                                  && sub.StartDate.Value.Month == monthSheet.Date.Month)
                        {
                            subExpense.BuyDate = sub.StartDate.Value;
                            monthSheet.Categories.Where(c => c.Id == subscriptionsCategory.Id).First().Expenses.Add(subExpense);
                        }
                        else
                        {
                            switch (sub.RecurringUnit)
                            {
                                case RecurringUnitEnum.Monthly:
                                    if (IsRecurringMonth(sub.RenewalDate, sub.RecurringInterval, monthSheet.Date))
                                    {
                                        subExpense.BuyDate = new DateTimeOffset(monthSheet.Date.Year,
                                                                                monthSheet.Date.Month,
                                                                                sub.RenewalDate.Day,
                                                                                12, 12, 12, TimeSpan.Zero);
                                        monthSheet.Categories.Where(c => c.Id == subscriptionsCategory.Id).First().Expenses.Add(subExpense);
                                    }
                                    break;
                                case RecurringUnitEnum.Yearly:
                                    if (IsRecurringYear(sub.RenewalDate, sub.RecurringInterval, monthSheet.Date)
                                     && monthSheet.Date.Month == sub.RenewalDate.Month)
                                    {
                                        subExpense.BuyDate = new DateTimeOffset(monthSheet.Date.Year,
                                                                                monthSheet.Date.Month,
                                                                                sub.RenewalDate.Day,
                                                                                12, 12, 12, TimeSpan.Zero);
                                        monthSheet.Categories.Where(c => c.Id == subscriptionsCategory.Id).First().Expenses.Add(subExpense);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (monthSheet.Categories.Any(c => c.Name == "Subscriptions" && c.Expenses.Count == 0))
                        {
                            monthSheet.Categories.RemoveAll(c => c.Name == "Subscriptions");
                        }
                    }
                    #endregion
                }
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

        #region Helpers
        public static bool IsRecurringMonth(DateTimeOffset renewalDate, int interval, DateTimeOffset monthSheetDate)
        {
            if (interval <= 0) return false;
            if (interval == 1) return true;

            //normalize dates to the first day to do comparison
            var currentRenewal = new DateTimeOffset(renewalDate.Year, renewalDate.Month, 1, 0, 0, 0, renewalDate.Offset);
            var targetMonth = new DateTimeOffset(monthSheetDate.Year, monthSheetDate.Month, 1, 0, 0, 0, monthSheetDate.Offset);

            if (targetMonth < currentRenewal) return false;

            //add interval until targetMonth checks or passes
            while (currentRenewal <= targetMonth)
            {
                if (currentRenewal.Year == targetMonth.Year 
                && currentRenewal.Month == targetMonth.Month)
                {
                    return true;
                }
                currentRenewal = currentRenewal.AddMonths(interval);
            }
            return false;
        }

        public static bool IsRecurringYear(DateTimeOffset renewalDate, int interval, DateTimeOffset monthSheetDate)
        {
            if (interval <= 0) return false;

            //normalize dates to the first day to do comparison
            var currentRenewal = new DateTimeOffset(renewalDate.Year, 1, 1, 0, 0, 0, renewalDate.Offset);
            var targetYear = new DateTimeOffset(monthSheetDate.Year, 1, 1, 0, 0, 0, monthSheetDate.Offset);

            if (targetYear < currentRenewal) return false;

            //add interval until targetYear checks or passes
            while (currentRenewal <= targetYear)
            {
                if (currentRenewal.Year == targetYear.Year)
                {
                    return true;
                }
                currentRenewal = currentRenewal.AddYears(interval);
            }
            return false;
        }
        #endregion
    }
}
