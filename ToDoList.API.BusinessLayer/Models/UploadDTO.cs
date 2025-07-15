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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string CreatedDate { get; set; }
        public int IdOwner { get; set; }
        public string EmailOwner { get; set; }
        public string Text { get; set; }
        public int IdTask { get; set; }

    }
}
