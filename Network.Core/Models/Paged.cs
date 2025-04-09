using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class Paged<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; private set; }
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public int MinPage { get; private set; } = 1;
        public int MaxPage { get; private set; }

        public Paged(int countItems, int pageIndex, int pageSize, int totalLimitItems)
        {
            if (pageSize < MinPage)
            {
                throw new Exception($"*** Number of items per page must > 0! ***");
            }
            int totalItems = totalLimitItems <= countItems ? totalLimitItems : countItems;
            TotalItems = totalItems;
            PageIndex = pageIndex;
            PageSize = pageSize;
            MaxPage = CalculateTotalPages(totalItems, pageSize);
        }
        public Paged(IQueryable<object> query, int pageIndex, int pageSize, int totalLimitItems)
        {
            if (pageSize < MinPage)
            {
                throw new Exception($"*** Number of items per page must > 0! ***");
            }
            int countItems = query.Count();
            int totalItems = totalLimitItems >= countItems ? totalLimitItems : countItems;
            TotalItems = totalItems;
            PageIndex = pageIndex;
            PageSize = pageSize;
            MaxPage = CalculateTotalPages(totalItems, pageSize);
        }
        private int CalculateTotalPages(int totalItems, int pageSize)
        {
            int totalPages = totalItems / pageSize;

            if (totalItems % pageSize != 0)
            {
                totalPages++;
            }
            return totalPages;
        }
    }
}
