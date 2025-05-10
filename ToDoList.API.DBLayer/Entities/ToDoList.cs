using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ToDoListInfo.API.DBLayer.Entities
{
    public class ToDoList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Task { get; set; }

        [ForeignKey("Priority")]
        public short Priority { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("IdOwner")]
        public int IdOwner { get; set; }

        public DateTime DueDate { get; set; }

    }
}
