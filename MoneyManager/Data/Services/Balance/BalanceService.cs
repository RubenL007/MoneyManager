using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Data.Models.Balance;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class BalanceService : IBalance
    {
        private readonly IMongoCollection<BalanceModel> _balancesCollection;
        private readonly IUserAuthentication _userAuthentication;
        private IAccount AccountService;

        public BalanceService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication, IAccount accountService)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _balancesCollection = database.GetCollection<BalanceModel>("Balances");
            _userAuthentication = userAuthentication;
            AccountService = accountService;
        }

        #region CreateBalance(BalanceModel balance)
        public string CreateBalance(BalanceModel balance)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            //fix DateTimeOffset error in mongo
            var start = new DateTime(balance.Date.Year, balance.Date.Month, 1);
            var end = start.AddMonths(1);

            var balanceObj = _balancesCollection.Find(b => b.UserId == userId
                                                        && b.Id == balance.Id
                                                                || (b.UserId == userId && b.Date >= start && b.Date < end)).FirstOrDefault();

            if (balanceObj == null)
            {
                List<AccountModel> accounts = AccountService.SearchAccounts();
                balance.Accounts = accounts;
                balance.UserId = userId!;
                _balancesCollection.InsertOne(balance);
                return "Balance saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateBalance(BalanceModel balance)
        public string UpdateBalance(BalanceModel balance)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_balancesCollection.Find(a => a.UserId == userId
                                           && a.Id == balance.Id).Any())
            {
                _balancesCollection.ReplaceOne(a => a.UserId == userId 
                                                 && a.Id == balance.Id, balance);
                return "Balance updated sucessfully.";
            }
            else
            {
                return "The balance could not be found.";
            }
        }
        #endregion

        #region BalanceModel GetBalance(Guid id)
        public BalanceModel GetBalance(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _balancesCollection.Find(a => a.UserId == userId
                                              && a.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<BalanceModel> SearchBalances()
        public List<BalanceModel> SearchBalances()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _balancesCollection.Find(a => a.UserId == userId).ToList();
        }
        #endregion

        #region DeleteBalance(Guid id)
        public string DeleteBalance(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_balancesCollection.Find(a => a.UserId == userId 
                                           && a.Id == id).Any())
            {
                _balancesCollection.DeleteOne(a => a.UserId == userId 
                                                && a.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The balance could not be found.";
            }
        } 
        #endregion
    }
}
