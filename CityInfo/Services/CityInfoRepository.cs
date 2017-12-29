using CityInfo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {

        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {

            if (includePointsOfInterest)
                //return _context.Cities.Where(c => c.Id == cityId).Include(c => c.PointOfInterests).SingleOrDefault(c => c.Id == cityId);
                return _context.Cities.Include(c => c.PointsOfInterest).FirstOrDefault(c => c.Id == cityId);

            return _context.Cities.SingleOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterests.SingleOrDefault(c => c.CityId == cityId && c.Id == pointOfInterestId);
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestsForCity(int cityId)
        {
            return _context.PointOfInterests.Where(c => c.CityId == cityId).ToList();
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest dto)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(dto);

        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePointOfInterestForCity(int cityId, int poiId, JsonPatchDocument<PointOfInterest> dto)
        {
            var tempUpdateObject = GetPointOfInterestForCity(cityId, poiId);
            dto.ApplyTo(tempUpdateObject);
        }

        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest)
        {

            _context.PointOfInterests.Remove(pointOfInterest);

        }
    }
}
