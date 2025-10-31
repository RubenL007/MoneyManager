using MoneyManager.Data.Models.Investments;

namespace MoneyManager.Data.Interface.Investments
{
    public interface IInvestment
    {
        string CreateInvestment(InvestmentModel investment);
        string UpdateInvestment(InvestmentModel investment);
        InvestmentModel GetInvestment(Guid id);
        List<InvestmentModel> SearchInvestments();
        string DeleteInvestment(Guid id);
    }
}
