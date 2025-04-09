using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Helpers
{
    public class NumberHelpers
    {
        public static bool IsNumber(string Value)
        {
            try
            {
                int n;
                bool isNumeric = int.TryParse(Value, out n);
                return isNumeric;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsDecimal(string Value)
        {
            try
            {
                decimal n;
                bool isNumeric = decimal.TryParse(Value, out n);
                return isNumeric;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNumberRegex(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"\d");
        }
    }
}
