using System.ComponentModel.DataAnnotations;

namespace CityInfo.Dtos
{
    public class PointsOfInterestCreateDto
    {
        [Required(ErrorMessage = "The name field must be provided")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
