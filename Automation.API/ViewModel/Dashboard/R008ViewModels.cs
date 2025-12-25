using System;
using System.Collections.Generic;

namespace Network.API.ViewModel.Dashboard
{
    // Response for R008 Dashboard Statistics
    public class R008DashboardResponse
    {
        public int TotalCells { get; set; }
        public int CellsExecuted { get; set; }
        public int CellsNotExecuted { get; set; }
        public int CellsExecutedOff { get; set; }
        public int CellsExecutedOn { get; set; }
        public double TotalExecutionHours { get; set; }
        public List<R008_DailyStatistics> DailyStatistics { get; set; }
    }
    
    // Statistics by date/week/month
    public class R008_DailyStatistics
    {
        public DateTime Date { get; set; }
        public int TotalCells { get; set; }
        public int CellsExecuted { get; set; }
        public int CellsNotExecuted { get; set; }
        public int CellsExecutedOff { get; set; }
        public int CellsExecutedOn { get; set; }
        public double TotalExecutionHours { get; set; }
    }
    
    // Request model for date range queries
    public class R008DateRangeRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeType { get; set; } // "day", "week", "month"
    }
    
    // Response for paginated data
    public class R008PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
