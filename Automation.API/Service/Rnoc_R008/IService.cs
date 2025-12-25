using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Service.Rnoc_R008
{
    public interface IService
    {
        // Dashboard methods for different time periods
        Task<R008DashboardResponse> GetDashboardByDayAsync(DateTime startDate, DateTime endDate);
        Task<R008DashboardResponse> GetDashboardByWeekAsync(DateTime startDate, DateTime endDate);
        Task<R008DashboardResponse> GetDashboardByMonthAsync(DateTime startDate, DateTime endDate);
        
        // Get all records with pagination
        Task<R008PagedResponse<R008_RunScheduler>> GetSchedulerRecordsPagedAsync(DateTime startDate, DateTime endDate, int page, int pageSize);
        
        // Get statistics by specific date
        Task<R008DashboardResponse> GetStatisticsByDateAsync(DateTime date);
        
        // Get records by cell name
        Task<List<R008_RunScheduler>> GetRecordsByCellNameAsync(string cellName, DateTime startDate, DateTime endDate);
        
        // Export methods
        Task<string> ExportSchedulerRecordsToCsvAsync(DateTime startDate, DateTime endDate);
    }
}
