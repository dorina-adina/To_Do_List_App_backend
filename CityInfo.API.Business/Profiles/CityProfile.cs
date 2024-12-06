using AutoMapper;
using CityInfo.API.Businsess_Layer.Models;
using CityInfo.API.Data_Access_Layer.Entities;

namespace CityInfo.API.Businsess_Layer.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointsOfInterestDto>();
            CreateMap<City, CityDto>();

        }
    }
}
