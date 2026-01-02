namespace MoneyManager.Data.Models.Balance
{
    public class ChartAccountModel
    {
        public string AccountName { get; set; } = string.Empty;
        public Guid AccountId { get; set; } = Guid.Empty;
        public List<ChartAccountBalanceModel> AccountBalances { get; set; } = new();
    }
}
