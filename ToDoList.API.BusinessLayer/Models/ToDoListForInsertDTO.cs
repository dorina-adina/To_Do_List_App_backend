using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.BusinessLayer.Models
{
    public class ToDoListForInsertDTO
    {
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
