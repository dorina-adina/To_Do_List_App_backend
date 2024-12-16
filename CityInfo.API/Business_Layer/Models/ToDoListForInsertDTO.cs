namespace CityInfo.API.Business_Layer.Models
{
    public class ToDoListForInsertDTO
    {
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? Createdby { get; set; }
    }
}
