namespace CityInfo.API.Businsess_Layer.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }



        //    public int NumberPointsOfInterest
        //    {
        //        get
        //        {
        //            return PointOfInterest.Count;
        //        }
        //    }

        //    public ICollection<PointOfInterestDto> PointOfInterest { get; set; }
        //        = new List<PointOfInterestDto>();
        //}
    }
}
