using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Network.API.Service.Rnoc_R009
{
    public interface IService
    {
        // Huawei methods
        Task<List<Hw_BtsData>> GetBtsDataByDateAsync(DateTime date);
        Task<List<Hw_BtsData>> GetBtsDataByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Nokia 4G methods
        Task<List<Nokia_BtsData>> GetNokiaBtsDataByDateAsync(DateTime date);
        Task<List<Nokia_BtsData>> GetNokiaBtsDataByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Nokia 5G methods
        Task<List<Nokia_BtsData5G>> GetNokiaBtsData5GByDateAsync(DateTime date);
        Task<List<Nokia_BtsData5G>> GetNokiaBtsData5GByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // ZTE methods
        Task<List<Zte_BtsData>> GetZteBtsDataByDateAsync(DateTime date);
        Task<List<Zte_BtsData>> GetZteBtsDataByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Ericsson methods
        Task<List<Ericsson_BtsData>> GetEricssonBtsDataByDateAsync(DateTime date);
        Task<List<Ericsson_BtsData>> GetEricssonBtsDataByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Dashboard 4G methods
        Task<Dashboard4GResponse> GetDashboard4GDataAsync(DateTime date);
        
        // Dashboard 5G methods
        Task<Dashboard5GResponse> GetDashboard5GDataAsync(DateTime date);
        
        // Provincial Report methods
        Task<List<ProvincialData>> GetProvincialReport4GAsync(DateTime date);
        Task<List<ProvincialData5G>> GetProvincialReport5GAsync(DateTime date);
        Task<object> GetProvincialReportAllAsync(DateTime date);
    }
} 