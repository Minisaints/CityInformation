using AutoMapper;
using CityInfo.Dtos;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CityInfo.Controllers
{
    // Attribute routing
    [Route("api/cities/")]
    public class PointsOfInterestController : Controller
    {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _repository;

        // Dependancy Injection
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository repository)
        {
            _logger = logger;
            _mailService = mailService;
            _repository = repository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public ActionResult GetPointsOfInterest(int cityId)
        {

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var poiEntity = _repository.GetPointOfInterestsForCity(cityId);

            var result = Mapper.Map<IEnumerable<PointsOfInterestDto>>(poiEntity);

            return Ok(result);

        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public ActionResult GetPointOfInterest(int cityId, int id)
        {

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
                return NotFound();

            var result = Mapper.Map<PointsOfInterestDto>(pointOfInterest);

            return Ok(result);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public ActionResult CreatePointOfInterest(int cityId, [FromBody]PointsOfInterestCreateDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var poi = Mapper.Map<PointOfInterest>(dto);

            _repository.AddPointOfInterestForCity(cityId, poi);


            if (!_repository.Save())
            {
                return StatusCode(500, "A problem occurred when saving the context.");
            }

            var result = Mapper.Map<PointsOfInterestDto>(poi);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                id = result.Id
            }, result);

        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public ActionResult UpdatePointOfInterest(int cityId, int id, [FromBody]JsonPatchDocument<PointsOfInterestUpdateDto> patch)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var temp = Mapper.Map<JsonPatchDocument<PointOfInterest>>(patch);

            _repository.UpdatePointOfInterestForCity(cityId, id, temp);

            if (!_repository.Save())
            {
                return StatusCode(500, "A problem occurred when saving the context.");
            }


            return Ok(temp);
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public ActionResult DeletePointOfInterest(int cityId, int id)
        {

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterestForCity(pointOfInterest);

            if (!_repository.Save())
            {
                return StatusCode(500, "A problem occurred when saving the context.");
            }

            _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted.");

            return NoContent();
        }
    }
}