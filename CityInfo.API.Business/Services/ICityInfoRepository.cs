using CityInfo.API.Data_Access_Layer.Entities;

namespace CityInfo.API.Businsess_Layer.Services
{
    public interface ICityInfoRepository
    {
        //IEnumerable<City> GetCities();
        //IQueryable<City> GetCities();

        Task<IEnumerable<City>> GetCitiesAsync();

        //Task<IEnumerable<City>> GetCityAsync(string? name);
        Task<(IEnumerable<City>, PaginationMetadata)> GetCityAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<City?> GetCityAsync(int cityId, bool includePointOfInterest);
        Task<bool> CityExistsAsync(int cityId);

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);

        Task<PointOfInterest> GetPointOfInterestAsync(int cityId, int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityid, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync();


    }
}
