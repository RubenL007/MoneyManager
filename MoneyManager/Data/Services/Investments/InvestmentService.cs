using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface.Investments;
using MoneyManager.Data.Models.Investments;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services.Investments
{
    public class InvestmentService : IInvestment
    {
        private readonly IMongoCollection<InvestmentModel> _investmentsCollection;
        private readonly IUserAuthentication _userAuthentication;

        public InvestmentService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _investmentsCollection = database.GetCollection<InvestmentModel>("Investments");

            _userAuthentication = userAuthentication;
        }

        #region CreateInvestment(InvestmentModel investment)
        public string CreateInvestment(InvestmentModel investment)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var investmentObj = _investmentsCollection.Find(a => a.UserId == userId
                                                              && a.Id == investment.Id).FirstOrDefault();
            if (investmentObj == null && !string.IsNullOrWhiteSpace(investment.Name))
            {
                investment.UserId = userId!;
                _investmentsCollection.InsertOne(investment);
                return "Investment saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateInvestment(InvestmentModel investment)
        public string UpdateInvestment(InvestmentModel investment)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_investmentsCollection.Find(a => a.UserId == userId
                                              && a.Id == investment.Id).Any())
            {
                _investmentsCollection.ReplaceOne(a => a.UserId == userId
                                                    && a.Id == investment.Id, investment);
                return "Investment updated sucessfully.";
            }
            else
            {
                return "The investment could not be found.";
            }
        }
        #endregion

        #region InvestmentModel GetInvestment(Guid id)
        public InvestmentModel GetInvestment(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _investmentsCollection.Find(a => a.UserId == userId
                                                 && a.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<InvestmentModel> SearchInvestments()
        public List<InvestmentModel> SearchInvestments()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _investmentsCollection.Find(a => a.UserId == userId).ToList();
        }
        #endregion

        #region DeleteInvestment(Guid id)
        public string DeleteInvestment(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_investmentsCollection.Find(a => a.UserId == userId
                                              && a.Id == id).Any())
            {
                _investmentsCollection.DeleteOne(a => a.UserId == userId
                                                   && a.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The Investment could not be found.";
            }
        }
        #endregion
    }
}
