﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyManager.Data.Models
{
    public class CategoryBaseModel
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
    }
}
