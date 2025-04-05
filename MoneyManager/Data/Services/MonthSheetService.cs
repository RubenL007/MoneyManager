﻿using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class MonthSheetService : IMonthSheet
    {
        private readonly IMongoCollection<MonthSheetModel> _monthsCollection;
        private readonly IUserAuthentication _userAuthentication;

        public MonthSheetService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _monthsCollection = database.GetCollection<MonthSheetModel>("MonthsSheets");

            _userAuthentication = userAuthentication;
        }

        #region CreateMonthSheet(MonthSheetModel monthSheet)
        public string CreateMonthSheet(MonthSheetModel monthSheet)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var monthSheetObj = _monthsCollection.Find(m => m.UserId == userId
                                                       && m.Id == monthSheet.Id
                                                            || (m.UserId == userId
                                                            && m.Date.Year == monthSheet.Date.Year
                                                            && m.Date.Month == monthSheet.Date.Month)).FirstOrDefault();
            if (monthSheetObj == null)
            {
                monthSheet.UserId = userId!;
                _monthsCollection.InsertOne(monthSheet);
                return "Month Sheet saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateMonthSheet(MonthSheetModel monthSheet)
        public string UpdateMonthSheet(MonthSheetModel monthSheet)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_monthsCollection.Find(m => m.UserId == userId
                                       && m.Id == monthSheet.Id).Any())
            {
                _monthsCollection.ReplaceOne(m => m.UserId == userId
                                             && m.Id == monthSheet.Id, monthSheet);
                return "Month Sheet updated sucessfully.";
            }
            else
            {
                return "The month sheet could not be found.";
            }
        }
        #endregion

        #region MonthSheetModel GetMonthSheet(Guid id)
        public MonthSheetModel GetMonthSheet(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _monthsCollection.Find(m => m.UserId == userId
                                          && m.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<MonthSheetModel> SearchMonthSheets()
        public List<MonthSheetModel> SearchMonthSheets()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _monthsCollection.Find(m => m.UserId == userId)
                                    .ToList();
        }
        #endregion

        #region DeleteMonthSheet(Guid id)
        public string DeleteMonthSheet(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_monthsCollection.Find(m => m.UserId == userId
                                       && m.Id == id).Any())
            {
                _monthsCollection.DeleteOne(m => m.UserId == userId
                                            && m.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The month sheet could not be found.";
            }
        } 
        #endregion
    }
}
