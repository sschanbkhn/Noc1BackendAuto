using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Service.Rnoc_R001
{
    public interface IService
    {
        // Dashboard methods
        Task<R001DashboardResponse> GetDashboardDataAsync(DateTime date);
        
        // Configured sites methods
        Task<List<R001_DataRuntime>> GetConfiguredSitesByDateAsync(DateTime date);
        Task<List<R001_DataRuntime>> GetConfiguredSitesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<(List<R001_DataRuntime> Data, int TotalCount)> GetConfiguredSitesByDatePagedAsync(DateTime date, int page, int pageSize);
        
        // Bad configuration methods
        Task<List<R001_DataRuntimeBad>> GetBadConfigurationsByDateAsync(DateTime date);
        Task<List<R001_DataRuntimeBad>> GetBadConfigurationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<(List<R001_DataRuntimeBad> Data, int TotalCount)> GetBadConfigurationsByDatePagedAsync(DateTime date, int page, int pageSize);
        
        // Detail methods with pagination
        Task<R001DetailResponse> GetCorrectConfigurationsAsync(R001DetailRequest request);
        Task<R001DetailResponse> GetIncorrectConfigurationsAsync(R001DetailRequest request);
        Task<R001DetailResponse> GetParameterDetailsAsync(R001DetailRequest request);
        
        // Statistics methods
        Task<R001Statistics> GetStatisticsAsync(DateTime date);
        Task<List<R001ParameterSummary>> GetParameterSummariesAsync(DateTime date);
        Task<int> GetTotalUniqueNECountAsync(DateTime date);
        
        // Export methods
        Task<string> ExportConfiguredSitesToCsvAsync(DateTime startDate, DateTime endDate);
        Task<string> ExportBadConfigurationsToCsvAsync(DateTime startDate, DateTime endDate);
        
        // Fix configuration methods (returns list because one config can generate multiple baseline types)
        Task<List<R001_SchedulerFixParameter>> FixSingleConfigurationAsync(R001_SchedulerFixParameter fixRequest);
        Task<List<R001_SchedulerFixParameter>> FixAllConfigurationsAsync(List<R001_SchedulerFixParameter> fixRequests);
        
        // Get fix parameters by date
        Task<(List<R001_SchedulerFixParameter> Data, int TotalCount)> GetFixParametersByDatePagedAsync(DateTime date, int page, int pageSize);
    }
}