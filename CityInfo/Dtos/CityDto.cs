using System.Collections.Generic;

namespace CityInfo.Dtos
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PointsOfInterestDto> PointsOfInterest { get; set; } = new List<PointsOfInterestDto>();

        public int NumberOfPointsOfInterest => PointsOfInterest.Count;
    }
}
