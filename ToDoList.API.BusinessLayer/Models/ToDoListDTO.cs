namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class ToDoListDTO
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
