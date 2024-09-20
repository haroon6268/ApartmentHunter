using Apartments.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;

namespace ApartmentsApi.Services{
    public class ListingService{
        private readonly IMongoCollection<Listing> _listingsCollection;

        public ListingService(IOptions<ApartmentsDatabaseSettings> settings){
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _listingsCollection = mongoDatabase.GetCollection<Listing>(settings.Value.ListingCollectionName);
        }

        public async Task<List<Listing>> GetAsync() => 
            await _listingsCollection.Find(x => true).ToListAsync();

        public async Task<Listing?> GetAsync(string id) => 
            await _listingsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        
        public async Task CreateAsync(Listing newListing) => 
            await _listingsCollection.InsertOneAsync(newListing);
        
        public async Task UpdateAsync(string id, Listing update) =>
            await _listingsCollection.ReplaceOneAsync(x => x.Id == id, update);
        
        public async Task RemoveAsync(string id) =>
            await _listingsCollection.DeleteOneAsync(x => x.Id == id);
    
    }
}