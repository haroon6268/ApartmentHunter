using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Apartments.Models{
    public class User{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id{get;set;} = null!;
        public string email{get;set;} = null!;
        public string? password{get;set;} = null!;

        public string? name{get;set;} = null!;
        public List<string> listings{get;set;} = new List<string>{};
    }
}