using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class AccountService : IAccount
    {
        private readonly IMongoCollection<AccountModel> _accountsCollection;
        private readonly IUserAuthentication _userAuthentication;

        public AccountService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _accountsCollection = database.GetCollection<AccountModel>("Accounts");

            _userAuthentication = userAuthentication;
        }

        #region CreateAccount(AccountModel account)
        public string CreateAccount(AccountModel account)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var accountObj = _accountsCollection.Find(a => a.UserId == userId
                                                      && a.Id == account.Id).FirstOrDefault();
            if (accountObj == null)
            {
                account.UserId = userId!;
                _accountsCollection.InsertOne(account);
                return "Account saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateAccount(AccountModel account)
        public string UpdateAccount(AccountModel account)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_accountsCollection.Find(a => a.UserId == userId
                                         && a.Id == account.Id).Any())
            {
                _accountsCollection.ReplaceOne(a => a.UserId == userId 
                                               && a.Id == account.Id, account);
                return "Account updated sucessfully.";
            }
            else
            {
                return "The account could not be found.";
            }
        }
        #endregion

        #region AccountModel GetAccount(Guid id)
        public AccountModel GetAccount(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _accountsCollection.Find(a => a.UserId == userId
                                            && a.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<AccountModel> SearchAccounts()
        public List<AccountModel> SearchAccounts()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _accountsCollection.Find(a => a.UserId == userId).ToList();
        }
        #endregion

        #region DeleteAccount(Guid id)
        public string DeleteAccount(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_accountsCollection.Find(a => a.UserId == userId 
                                         && a.Id == id).Any())
            {
                _accountsCollection.DeleteOne(a => a.UserId == userId 
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
