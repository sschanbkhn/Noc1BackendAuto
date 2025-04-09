using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.Core.Helpers
{
    public static class DateTimeHelpers
    {
        public static string CurrentDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        public static string CurrentDateTime()
        {
            return DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        }
        public static long Date2Number(DateTime date)
        {
            int year = date.Year, month = date.Month, day = date.Day;
            string result = string.Empty;
            result += year;
            if (month.ToString().Length == 1)
                result += "0" + month;
            else
                result += month;

            if (day.ToString().Length == 1)
                result += "0" + day;
            else
                result += day;

            return long.Parse(result);
        }
        public static long Datetime2Number(DateTime dateTime)
        {
            int year = dateTime.Year, month = dateTime.Month, day = dateTime.Day, hour = dateTime.Hour, minute = dateTime.Minute, second = dateTime.Second;
            string result = string.Empty;
            result += year;
            if (month.ToString().Length == 1)
                result += "0" + month;
            else
                result += month;

            if (day.ToString().Length == 1)
                result += "0" + day;
            else
                result += day;

            if (hour.ToString().Length == 1)
                result += "0" + hour;
            else
                result += hour;

            if (minute.ToString().Length == 1)
                result += "0" + minute;
            else
                result += minute;

            if (second.ToString().Length == 1)
                result += "0" + second;
            else
                result += second;

            return long.Parse(result);
        }
        public static DateTime? Number2Date(long numDate)
        {
            if (numDate.ToString().Length > 8)
                return null;
            string year = numDate.ToString().Substring(0, 4);
            string month = numDate.ToString().Substring(4, 2);
            string day = numDate.ToString().Substring(6, 2);
            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }
        public static DateTime? Number2Datetime(long numDateTime)
        {
            if (numDateTime.ToString().Length > 14)
                return null;
            string year = numDateTime.ToString().Substring(0, 4);
            string month = numDateTime.ToString().Substring(4, 2);
            string day = numDateTime.ToString().Substring(6, 2);            
            string hour = numDateTime.ToString().Substring(8, 2);
            string minute = numDateTime.ToString().Substring(10, 2);
            string second = numDateTime.ToString().Substring(12, 2);
            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));            
        }
    }
}
