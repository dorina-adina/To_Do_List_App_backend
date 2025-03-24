using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.Data_AccessLayer.Entities
{
    public class Upload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("IdOwner")]
        public int IdOwner { get; set; }
        public string EmailOwner { get; set; }
        public string InfoPath { get; set; }



    }
}
