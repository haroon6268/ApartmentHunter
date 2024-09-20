using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Apartments.Models{
    public class Listing{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get;set;} = null!;
        public string? Creator{get;set;} = null!;
        public string address {get;set;} = null!;
        
        public List<string> imgs {get;set;} = null!;
        public double? lat{get;set;} = null!;
        public double? lng{get;set;} = null!;
        public string description {get;set;} = null!;
        public List<string> amenities{get;set;} = null!;
        public int? beds{get;set;} = null!;
        // public string name{get;set;} = null!;
        public int? SquareFootage {get;set;} = null!;
        public int? Bathrooms{get;set;} = null!;
        public int? Rent{get;set;} = null!;
        public int? SecurityDeposit{get;set;} = null!;
        // public string tags{get;set;} = null!;


    }
}