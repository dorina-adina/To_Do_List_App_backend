using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfo.API.Data_AccessLayer.Helpers
{
    public static class DateAndTimeHelper
    {
        public static string DateFormat(DateTime initialDate)
        {
            var formattedDate = initialDate.ToString("dd-MM-yyyy hh:mm");
            return formattedDate;
        }

        public static DateTime ReverseDate(string initialDate)
        {
            var initialFormattedDate = initialDate[4] + initialDate[5] + initialDate[3] + initialDate[1] + initialDate[2] + initialDate[3] + initialDate.Substring(6);
            var formattedDate = DateTime.Parse(initialFormattedDate);
            return formattedDate;
        }
    }
}
