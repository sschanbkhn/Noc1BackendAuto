using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Helpers
{
    public static class IEnumerableHelpers
    {
        public static IEnumerable<T> Paged<T>(this IEnumerable<T> source, int page, int pageSize, int totalLimitItems)
        {
            return source.Take(totalLimitItems).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }
    }
}
