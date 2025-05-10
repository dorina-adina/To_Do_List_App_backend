using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class UploadDTO
    {

        [ForeignKey("IdOwner")]
        public int IdOwner { get; set; }
        public string EmailOwner { get; set; }
        public string InfoPath { get; set; }

    }
}
