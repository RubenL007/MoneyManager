using Microsoft.Extensions.Options;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class SellerService : ISeller
    {
        private readonly IMongoCollection<SellerModel> _sellersCollection;
        private readonly IUserAuthentication _userAuthentication;

        public SellerService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IUserAuthentication userAuthentication)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _sellersCollection = database.GetCollection<SellerModel>("Sellers");

            _userAuthentication = userAuthentication;
        }

        #region CreateSeller(SellerModel seller)
        public string CreateSeller(SellerModel seller)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            var sellerObj = _sellersCollection.Find(s => s.UserId == userId
                                                    && s.Id == seller.Id).FirstOrDefault();
            if (sellerObj == null)
            {
                seller.UserId = userId!;
                _sellersCollection.InsertOne(seller);
                return "Seller saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }
        #endregion

        #region UpdateSeller(SellerModel seller)
        public string UpdateSeller(SellerModel seller)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_sellersCollection.Find(s => s.UserId == userId
                                        && s.Id == seller.Id).Any())
            {
                _sellersCollection.ReplaceOne(s => s.UserId == userId
                                              && s.Id == seller.Id, seller);
                return "Seller updated sucessfully.";
            }
            else
            {
                return "The seller could not be found.";
            }
        }
        #endregion

        #region SellerModel GetSeller(Guid id)
        public SellerModel GetSeller(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _sellersCollection.Find(s => s.UserId == userId
                                           && s.Id == id).FirstOrDefault();
        }
        #endregion

        #region List<SellerModel> SearchSellers()
        public List<SellerModel> SearchSellers()
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            return _sellersCollection.Find(s => s.UserId == userId)
                                     .ToList();
        }
        #endregion

        #region DeleteSeller(Guid id)
        public string DeleteSeller(Guid id)
        {
            string? userId = _userAuthentication.GetCurrentUserId();

            if (_sellersCollection.Find(s => s.UserId == userId
                                        && s.Id == id).Any())
            {
                _sellersCollection.DeleteOne(s => s.UserId == userId
                                             && s.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The seller could not be found.";
            }
        } 
        #endregion
    }
}
