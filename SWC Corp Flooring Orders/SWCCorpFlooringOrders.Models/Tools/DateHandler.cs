using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCCorpFlooringOrders.Models.Tools {
    // Handles date formatting and validation
    public class DateHandler {
        // Formats a string into a string that DateTime can parse
        public static string FormatDate(string date) {
            return $"{date.Substring(0, 2)}/{date.Substring(2, 2)}/{date.Substring(4, 4)}";
        }

        // Checks if the date provided is a future date
        public static bool IsFutureDate(string date) {
            date = FormatDate(date);
            DateTime userDate = DateTime.Parse(date);
            return userDate > DateTime.Today ? true : false;
        }

        // Checks if the date provided is a valid date
        public static bool IsValidDate(string date) {
            return DateTime.TryParse(FormatDate(date), out DateTime blank);
        }
    }
}
