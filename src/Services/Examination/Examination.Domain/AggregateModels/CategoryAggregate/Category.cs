using Examination.Domain.SeedWork;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Domain.AggregateModels.CategoryAggregate
{
    [BsonIgnoreExtraElements]
    public class Category : Entity, IAggregateRoot
    {
      
        public Category(string id, string name, string urlPath) => (Id, Name, UrlPath) = (id, name, urlPath);
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("urlPath")]
        public string UrlPath { get; set; } = string.Empty ;
    }
}
