﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyManager.Data.Models
{
    public class ExpenseModel
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public SellerModel Seller { get; set; } = new();

        public DateTime BuyDate { get; set; } = DateTime.Today;

        public AccountModel Account { get; set; } = new();

        public double Spent { get; set; } = 0;
    }
}
