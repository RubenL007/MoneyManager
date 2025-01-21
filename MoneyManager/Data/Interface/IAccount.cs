using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface IAccount
    {
        string CreateAccount(AccountModel account);
        string UpdateAccount(AccountModel account);
        AccountModel GetAccount(Guid id);
        List<AccountModel> SearchAccounts();
        string DeleteAccount(Guid id);
    }
}