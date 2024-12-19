namespace CityInfo.API.Business_Layer.Models
{
    public class ToDoListForUpdateDTO
    {
        //public int Id { get; set; }
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
