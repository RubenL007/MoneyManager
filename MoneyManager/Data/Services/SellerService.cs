﻿using MoneyManager.Data.Interface;
using MoneyManager.Data.Models;
using MongoDB.Driver;

namespace MoneyManager.Data.Services
{
    public class SellerService : ISeller
    {
        private MongoClient _mongoClient = null!;
        private IMongoDatabase _database = null!;
        private IMongoCollection<SellerModel> _sellersCollection = null!;
        public SellerService()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("MoneyManager");
            _sellersCollection = _database.GetCollection<SellerModel>("Sellers");
        }

        public string CreateSeller(SellerModel seller)
        {
            var sellerObj = _sellersCollection.Find(x => x.Id == seller.Id).FirstOrDefault();
            if (sellerObj == null)
            {
                _sellersCollection.InsertOne(seller);
                return "Seller saved sucessfully.";
            }
            else
            {
                return "An error has ocurred.";
            }
        }

        public string UpdateSeller(SellerModel seller)
        {
            if (_sellersCollection.Find(x => x.Id == seller.Id).Any())
            {
                _sellersCollection.ReplaceOne(x => x.Id == seller.Id, seller);
                return "Seller updated sucessfully.";
            }
            else
            {
                return "The seller could not be found.";
            }
        }

        public SellerModel GetSeller(Guid id)
        {
            return _sellersCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<SellerModel> SearchSellers()
        {
            return _sellersCollection.Find(FilterDefinition<SellerModel>.Empty).ToList();
        }

        public string DeleteSeller(Guid id)
        {
            if (_sellersCollection.Find(x => x.Id == id).Any())
            {
                _sellersCollection.DeleteOne(x => x.Id == id);
                return "Deleted with success.";
            }
            else
            {
                return "The seller could not be found.";
            }
        }
    }
}
