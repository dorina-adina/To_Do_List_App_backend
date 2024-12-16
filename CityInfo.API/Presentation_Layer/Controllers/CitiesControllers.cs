using System.Text.Json;
using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Business_Layer.Models;
using CityInfo.API.Business_Layer.Services;
using CityInfo.API.Businsess_Layer.Models;
using CityInfo.API.Businsess_Layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Presentation_Layer.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    //[Route("api/[controller]")]
    [ApiVersion(1)]
    [ApiVersion(2)]


    public class CitiesControllers : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        //private readonly ToDoListRepo _toDoListRepo;

        //private readonly CitiesDataStore _citiesDataStore;

        //[HttpGet]
        //public JsonResult GetCities()
        //{
        //    return new JsonResult(
        //        new List<object>
        //        {
        //            new{id = 1, Name = "Cluj"}
        //        }
        //        );
        //}        
        //[HttpGet("api/cities"]

        //public CitiesControllers(CitiesDataStore citiesDataStore)
        //{
        //    _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        //}

        public CitiesControllers(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            //_toDoListRepo = toDoListRepo ??
            //    throw new ArgumentNullException(nameof(toDoListRepo));
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetLists()
        //{
        //    var listsEntities = await _toDoListRepo.GetListsAsync();

        //    return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(listsEntities));

       
        //}

        //[HttpGet]
        //public async  Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        //    [FromQuery] string? name)
        //{
        //    var cityEntities = await _cityInfoRepository.GetCityAsync(name);

        //    return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

        //    //var results = new List<CityWithoutPointsOfInterestDto>();

        //    //foreach(var cityEntity in cityEntities)
        //    //{
        //    //    results.Add(new CityWithoutPointsOfInterestDto{
        //    //        Id = cityEntity.Id,
        //    //        Description = cityEntity.Description, 
        //    //        Name = cityEntity.Name
        //    //    });
        //    //}
        //    //return Ok(results);

        //    //var cities = _citiesDataStore.Cities as IEnumerable<CityDto>;
        //    //return Ok(cities);

        //    //var temp = new JsonResult(CitiesDataStore.Current.Cities);
        //    //temp.StatusCode = 200;
        //    //return new JsonResult(CitiesDataStore.Current);
        //    //new List<object>
        //    //{
        //    //    new { id = 1, Name = "Cluj"},
        //    //    new {id = 2, Name = "Oradea"}
        //    //});  
        //}


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            [FromQuery] string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCityAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));


            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));

        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includePointsOfInterest"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }
            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));

            //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

            //return Ok(cityToReturn);
            ////return new JsonResult (CitiesDataStore.Current.Cities
            ////    .FirstOrDefault(c => c.Id == id));

            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);
            //return Ok();
        }
    }
}
