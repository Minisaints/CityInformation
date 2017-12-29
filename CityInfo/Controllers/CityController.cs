using AutoMapper;
using CityInfo.Dtos;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.Controllers
{
    [Route("api/cities/")]
    public class CityController : Controller
    {

        private readonly ICityInfoRepository _repository;

        // Dependancy Injection
        public CityController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult GetCities()
        {
            var cityEntities = _repository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public ActionResult GetCity(int id, bool includePointsOfInterest = false)
        {

            var city = _repository.GetCity(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithoutPoi = Mapper.Map<CityWithoutPointsDto>(city);

            return Ok(cityWithoutPoi);
        }

    }
}
