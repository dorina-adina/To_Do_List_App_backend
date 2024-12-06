using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CityInfo.API.Businsess_Layer.Models;

namespace CityInfo.API.Data_Access_Layer.Entities
{
    public class City
    {

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string Name { get; set; }

        //[MaxLength(200)]
        //public string? Description { get; set; }

        //public ICollection<PointOfInterest> PointsOfInterest { get; set; }
        //       = new List<PointOfInterest>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(50)]
        public string? Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();


        public City(string name)
        {
            Name = name;
        }
    }

}
