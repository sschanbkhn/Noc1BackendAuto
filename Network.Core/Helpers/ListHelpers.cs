using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Core.Helpers
{
    public static class ListHelpers
    {
        public static List<T> Combines<T>(this List<T> collection1, List<T> collection2)
        {
            collection1.AddRange(collection2);
            return collection1;
        }

        public static ICollection<T> Combines<T>(this ICollection<T> collection1, ICollection<T> collection2)
        {
            var list = new List<T>();
            list.AddRange(collection1);
            list.AddRange(collection2);
            return list;
        }
        public static string ConcatStrings(List<string> items)
        {
            string result = string.Empty;
            if(items != null && items.Count > 0)
            {
                result = string.Join(",", items);
            }                
            return result;
        }
    }
}
