using System.Collections.Generic;
using System.Linq;

namespace CityInfo.Models
{
    public static class CityInfoContextExtensions
    {

        public static void EnsureSeedData(this CityInfoContext context)
        {
            if (context.Cities.Any())
                return;

            var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "The one with the big park",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "The most visited park"
                        },
                        new PointOfInterest()
                        {
                            Name = "Johnson Park",
                            Description = "Big black park"
                        },
                    }
                },
                new City()
                {
                    Name = "London",
                    Description = "The best place ever",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Big Wheel",
                            Description = "Londons big wheel lid"
                        },
                        new PointOfInterest()
                        {
                            Name = "Treasure island",
                            Description = "A place to find treasure"
                        }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
