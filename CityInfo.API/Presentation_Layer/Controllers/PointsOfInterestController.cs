using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Businsess_Layer.Models;
using CityInfo.API.Businsess_Layer.Services;
using CityInfo.API.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Presentation_Layer.Controllers
{
    [Route("api/v{vesion:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiController]
    [ApiVersion(2)]

    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;
        //private readonly CitiesDataStore _citiesDataStore;

        //constructor injection
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

            //HttpContext.RequestServices.GetServices()
            //_citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault();
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository
                .GetPointsOfInterestAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));


            //var pointsOfInterestForCity = await _cityInfoRepository
            //    .GetPointsOfInterestAsync(cityId);


            //throw new Exception("Exc");

            //try
            //{
            //    //throw new Exception("Exc");
            //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasn't found");
            //        return NotFound();
            //    }

            //    return Ok(city.PointsOfInterest);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogCritical($"Exception", ex);
            //    return StatusCode(500, "Problem");
            //}
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            //var city = _citiesDataStore.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {

                return NotFound();
            }


            // find point of interest
            //var pointOfInterest = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointOfInterestId);

            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
           int cityId,
           PointOfInterestForCreationDto pointOfInterest)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {

                return NotFound();
            }

            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            // demo purposes - to be improved
            //var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(
            //                 c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            //city.PointsOfInterest.Add(finalPointOfInterest);

            //*await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            //*await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                 new
                 {
                     cityId,
                     pointOfInterestId = finalPointOfInterest.Id
                 },
                 createdPointOfInterestToReturn);
        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfIntrestForUpdateDto pointOfInterest)
        {
            //var city = _citiesDataStore.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {

                return NotFound();
            }

            // find point of interest
            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointOfInterestId);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            //pointOfInterestEntity.Name = pointOfInterest.Name;
            //pointOfInterestEntity.Description = pointOfInterest.Description;

            return NoContent();
        }


        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfIntrestForUpdateDto> patchDocument)
        {
            //var city = _citiesDataStore.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {

                return NotFound();
            }

            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointOfInterestId);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }


            //var pointOfInterestToPatch =
            //       new PointOfIntrestForUpdateDto()
            //       {
            //           Name = pointOfInterestFromStore.Name,
            //           Description = pointOfInterestFromStore.Description
            //       };

            var pointOfInterestToPatch = _mapper.Map<PointOfIntrestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();


            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            //var city = _citiesDataStore.Cities
            //    .FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {

                return NotFound();
            }

            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointOfInterestId);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} was deleted");
            return NoContent();
        }

    }
}



