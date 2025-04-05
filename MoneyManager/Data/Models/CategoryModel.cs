namespace MoneyManager.Data.Models
{
    public class CategoryModel : CategoryBaseModel
    {
        public List<ExpenseModel> Expenses { get; set; } = new();

        public double TotalSpent => Expenses.Sum(x => x.Spent);
    }
}
