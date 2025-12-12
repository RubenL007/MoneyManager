namespace MoneyManager.Data.Models.Subscriptions
{
    public enum RecurringUnitEnum
    {
        //TODO: rework logic about it, weekly needs to look at the day of the week (friday), not the number of the day or it wont be 100% correct
        //Weekly, 
        Monthly, 
        Yearly,
    }
}
