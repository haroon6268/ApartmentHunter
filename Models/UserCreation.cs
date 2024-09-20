using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Apartments.Models{
    public class UserCreation{
        public string? Id{get;set;} = null!;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email{get;set;} = null!;
        public string name{get;set;} = null!;
        public string password{get;set;} = null!;
        public string confirmPassword{get;set;} = null!;
    }
}