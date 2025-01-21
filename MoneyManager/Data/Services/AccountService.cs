using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class AccountService : IAccount
    {
        private MongoClient _mongoClient = null!;
        private IMongoDatabase _database = null!;
        private IMongoCollection<AccountModel> _accountsCollection = null!;
        public AccountService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("MoneyManager");
            _accountsCollection = _database.GetCollection<AccountModel>("Accounts");
        }

        public string CreateAccount(AccountModel account)
        {
            var accountObj = _accountsCollection.Find(x => x.Id == account.Id).FirstOrDefault();
            if (accountObj == null)
            {
                _accountsCollection.InsertOne(account);
                return "Account saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }

        public string UpdateAccount(AccountModel account)
        {
            if (_accountsCollection.Find(x => x.Id == account.Id).Any())
            {
                _accountsCollection.ReplaceOne(x => x.Id == account.Id, account);
                return "Account updated sucessfully.";
            }
            else
            {
                return "The account could not be found.";
            }
        }

        public AccountModel GetAccount(Guid id)
        {
            return _accountsCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<AccountModel> SearchAccounts()
        {
            return _accountsCollection.Find(FilterDefinition<AccountModel>.Empty).ToList();
        }

        public string DeleteAccount(Guid id)
        {
            if (_accountsCollection.Find(x => x.Id == id).Any())
            {
                _accountsCollection.DeleteOne(x => x.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The account could not be found.";
            }
        }
    }
}
