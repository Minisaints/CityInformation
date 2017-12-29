using CityInfo.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;

namespace CityInfo.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointOfInterestsForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        bool CityExists(int cityId);
        void AddPointOfInterestForCity(int cityId, PointOfInterest dto);
        bool Save();
        void UpdatePointOfInterestForCity(int cityId, int poiId, JsonPatchDocument<PointOfInterest> dto);
        void DeletePointOfInterestForCity(PointOfInterest pointOfInterest);
    }
}
