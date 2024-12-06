using AutoMapper;
using CityInfo.API.Businsess_Layer.Models;
using CityInfo.API.Data_Access_Layer.Entities;

namespace CityInfo.API.Businsess_Layer.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
            CreateMap<PointOfIntrestForUpdateDto, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfIntrestForUpdateDto>();

        }
    }
}
