using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface IMonthSheet
    {
        string CreateMonthSheet(MonthSheetModel monthSheet);
        string UpdateMonthSheet(MonthSheetModel monthSheet);
        MonthSheetModel GetMonthSheet(Guid id);
        List<MonthSheetModel> SearchMonthSheets();
        string DeleteMonthSheet(Guid id);
    }
}
