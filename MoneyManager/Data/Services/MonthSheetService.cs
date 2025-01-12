using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class MonthSheetService : IMonthSheet
    {
        private MongoClient _mongoClient = null!;
        private IMongoDatabase _database = null!;
        private IMongoCollection<MonthSheetModel> _Months = null!;
        public MonthSheetService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("MoneyManager");
            _Months = _database.GetCollection<MonthSheetModel>("MonthsSheets");
        }

        public string CreateMonthSheet(MonthSheetModel monthSheet)
        {
            var monthSheetObj = _Months.Find(x => x.Id == monthSheet.Id
                                            || (x.Date.Year == monthSheet.Date.Year && x.Date.Month == monthSheet.Date.Month)).FirstOrDefault();
            if (monthSheetObj == null)
            {
                _Months.InsertOne(monthSheet);
                return "Month Sheet saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }

        public string UpdateMonthSheet(MonthSheetModel monthSheet)
        {
            var monthSheetObj = _Months.Find(x => x.Id == monthSheet.Id).FirstOrDefault();
            if (monthSheetObj == null)
            {
                _Months.ReplaceOne(x => x.Id == monthSheet.Id, monthSheet);
                return "Month Sheet updated sucessfully.";
            }
            else
            {
                return "The month sheet could not be found.";
            }
        }

        public MonthSheetModel GetMonthSheet(Guid id)
        {
            return _Months.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<MonthSheetModel> SearchMonthSheetList()
        {
            return _Months.Find(FilterDefinition<MonthSheetModel>.Empty).ToList();
        }

        public string DeleteMonthSheet(Guid id)
        {
            _Months.DeleteOne(x => x.Id == id);
            return "Deleted with success.";
        }
    }
}
