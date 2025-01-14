using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListInfo.API.Data_AccessLayer.Entities
{
    public class ToDoList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Task { get; set; }

        public short Priority { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
