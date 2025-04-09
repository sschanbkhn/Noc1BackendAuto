using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Network.Core.Helpers
{
    public static class StringHelpers
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string TruncateWord(this string value, int wordLimit)
        {
            if (value == null || value.Length < wordLimit || value.IndexOf(" ", wordLimit) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", wordLimit));
        }
        public static string TruncateString(this string value, int charLimit)
        {
            if (value.Length <= charLimit)
                return value;
            if (value.Length > charLimit)
            {
                value = value.Substring(0, charLimit);
                int iSplitIndex = value.LastIndexOf(' ');
                if (iSplitIndex != -1)
                    value = value.Substring(0, iSplitIndex) + "...";
            }
            return value;
        }
        public static string RemoveAllWhitespace(this string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                return str.Replace(" ", "");
            }
            return str;
        }
        public static string RemoveSign4VietnameseString(this string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        public static T AsObject<T>(this string str)
        {            
            var obj = JsonConvert.DeserializeObject<T>(str);
            return obj;
        }
        public static string MultiInsert(this string str, string insertChar, int numChar, params int[] positions)
        {
            if (numChar == 0 || string.IsNullOrEmpty(insertChar) || positions.Count() == 0) return str;            
            StringBuilder sb = new StringBuilder(str.Length + (positions.Count() * insertChar.Length * numChar));
            var posLookup = new HashSet<int>(positions);
            for (int i = 0; i < str.Length; i++)
            {                
                if (posLookup.Contains(i))
                {
                    for (int j = 0; j < numChar; j++)
                    {
                        sb.Append(insertChar);
                    }
                }
                sb.Append(str[i]);
            }
            return sb.ToString();
        }
        public static string Left(this string value, int length)
        {
            length = Math.Abs(length);
            return string.IsNullOrEmpty(value) ? value : value.Substring(0, Math.Min(value.Length, length));
        }

        public static string Right(this string value, int length)
        {
            length = Math.Abs(length);
            return string.IsNullOrEmpty(value) ? value : value.Substring(value.Length - Math.Min(value.Length, length));
        }

        public static bool In(this string value, List<string> list)
        {
            return list.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        public static bool NotIn(this string value, List<string> list)
        {
            return !In(value, list);
        }

        public static bool EqualsIgnoreCase(this string source, string toCheck)
        {
            return string.Equals(source, toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static string ToBase64(this string src)
        {
            byte[] b = Encoding.UTF8.GetBytes(src);
            return Convert.ToBase64String(b);
        }

        public static string ToBase64(this string src, Encoding encoding)
        {
            byte[] b = encoding.GetBytes(src);
            return Convert.ToBase64String(b);
        }

        public static string FromBase64String(this string src)
        {
            byte[] b = Convert.FromBase64String(src);
            return Encoding.UTF8.GetString(b);
        }

        public static string FromBase64String(this string src, Encoding encoding)
        {
            byte[] b = Convert.FromBase64String(src);
            return encoding.GetString(b);
        }

        public static string Remove(this string source, params string[] removedValues)
        {
            removedValues.ToList().ForEach(x => source = source.Replace(x, ""));
            return source;
        }
        public static bool ContainsAny(this string input, IEnumerable<string> containsKeywords, StringComparison comparisonType)
        {
            return containsKeywords.Any(keyword => input.IndexOf(keyword, comparisonType) >= 0);
        }
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };
    }
}
