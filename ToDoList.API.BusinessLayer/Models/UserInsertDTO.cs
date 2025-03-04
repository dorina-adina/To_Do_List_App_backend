using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.BusinessLayer.Models
{
    public class UserInsertDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNr { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

    }
}
