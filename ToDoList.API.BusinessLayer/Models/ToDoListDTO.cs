using System.ComponentModel.DataAnnotations;

namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class ToDoListDTO
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public int IdOwner {  get; set; } 
        public DateTime? DueDate { get; set; }

    }
}
