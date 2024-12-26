using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CityInfo.API.Businsess_Layer.Models;


namespace CityInfo.API.Data_Access_Layer.Entities
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
