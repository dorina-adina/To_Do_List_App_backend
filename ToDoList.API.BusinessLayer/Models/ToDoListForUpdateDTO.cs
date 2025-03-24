using System.ComponentModel.DataAnnotations;

namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class ToDoListForUpdateDTO
    {
        public string Task { get; set; }
        public short Priority { get; set; }
        public string? Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IdOwner { get; set; }

        public DateTime? DueDate { get; set; }


    }
}
