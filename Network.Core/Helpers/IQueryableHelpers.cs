using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Helpers
{
    public static class IQueryableHelpers
    {
        public static IQueryable<T> Paged<T>(this IQueryable<T> source, int page, int pageSize, int totalLimitItems)
        {
            if(totalLimitItems == 0)
            {
                return source.Skip((page - 1) * pageSize).Take(pageSize);
            }    
            return source.Take(totalLimitItems).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
