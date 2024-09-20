namespace Apartments.Models;

public class ApartmentsDatabaseSettings{
    public string? ConnectionString{get;set;} = null!;
    public string? DatabaseName{get;set;} = null!;
    public string? ListingCollectionName{get;set;} = null!;
    public string UserCollectionName{get;set;} = null!;
}