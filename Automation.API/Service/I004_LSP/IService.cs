using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Network.API.Model;

namespace Network.API.Service
{
    public partial class I004_LSP
    {
        public interface IService
        {
            Task<List<LSPInternationalDataDto>> GetLSPInternationalDataAsync(DateTime fromDate, DateTime toDate);
            Task<List<RouterNodeDto>> GetPDataListAsync();
            Task<List<RouterNodeDto>> GetPOPDataListAsync();
            Task<RoutePCEPStatusDto> GetRoutePCEPStatusAsync();
            Task<LSPDelegatedStatusDto> GetLSPDelegatedStatusAsync();
            Task<LSPActionStatsDto> GetLSPActionStatsAsync(DateTime fromDate, DateTime toDate);
            Task<List<LSPBandwidthDto>> GetLSPBandwidthDataAsync(string[] fromIdNodes, string[] toIdNodes, DateTime fromDate, DateTime toDate);
            Task<List<LSPBandwidthDto>> GetBandwidthByPathAsync(string[] fromData, string[] toData, string timeRange, DateTime? fromDate = null, DateTime? toDate = null);
            Task<object> DebugDatabaseAsync();

        }
    }
}
