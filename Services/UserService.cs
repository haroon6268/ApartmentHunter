using Apartments.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;

namespace ApartmentsApi.Services{
    public class UserService{
        private readonly IMongoCollection<User> _userCollection;
        
        public UserService(IOptions<ApartmentsDatabaseSettings> settings){
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(settings.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() => await _userCollection.Find(x => true).ToListAsync();
        public async Task<User?> GetAsync(string email) => await _userCollection.Find(x => x.email == email).FirstOrDefaultAsync();
        public async Task CreateAsyc(User newUser) => await _userCollection.InsertOneAsync(newUser);
        public async Task UpdateAsync(string email, User newUser) => await _userCollection.ReplaceOneAsync(x => x.email == email, newUser);
        public async Task RemoveAsync(string id) => await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}