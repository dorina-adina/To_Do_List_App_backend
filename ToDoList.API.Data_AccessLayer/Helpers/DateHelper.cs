using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.Data_AccessLayer.Helpers
{
    public static class DateHelper
    {
        public static string DateFormat(DateTime initialDate)
        {
            var formattedDate = initialDate.ToString("dd-MM-yyyy");
            return formattedDate;
        }

        public static DateTime ReverseDate(string initialDate)
        {
            var formattedDate = DateTime.Parse(initialDate);
            return formattedDate;
        }
    }
}
