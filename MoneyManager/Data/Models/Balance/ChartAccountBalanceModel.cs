namespace MoneyManager.Data.Models.Balance
{
    public class ChartAccountBalanceModel
    {
        public double TotalValue { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
