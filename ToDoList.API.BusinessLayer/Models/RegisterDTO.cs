using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public string Pass {  get; set; }
    }
}
