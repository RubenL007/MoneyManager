using MoneyManager.Data.Models.Balance;

namespace MoneyManager.Data.Interface
{
    public interface IBalance
    {
        string CreateBalance(BalanceModel balance);
        string UpdateBalance(BalanceModel balance);
        BalanceModel GetBalance(Guid id);
        List<BalanceModel> SearchBalances();
        string DeleteBalance(Guid id);
    }
}