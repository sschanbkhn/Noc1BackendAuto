using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Interfaces;
using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;

namespace Network.API.Service.Rnoc_R001
{
    public class Service : Rnoc_R001.IService
    {
        private readonly RnocDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        
        // Standard parameter values for comparison
        private readonly Dictionary<string, string> _standardValues = new Dictionary<string, string>
        {
            { "utran_srvcc_switch", "ON" },
            { "utran_csfb_switch", "ON" },
            { "utran_flash_csfb_switch", "OFF" },
            { "geran_flash_csfb_switch", "OFF" },
            { "csfb_adaptive_blind_ho_switch", "OFF" },
            { "utran_csfb_steering_switch", "OFF" },
            { "idle_csfb_redirect_opt_switch", "ON" },
            { "dl_voip_bundling_switch", "ON" },
            { "ul_voip_pre_allocation_switch", "ON" },
            { "ul_voip_delay_sch_switch", "OFF" },
            { "ul_voip_load_based_sch_switch", "OFF" },
            { "ul_voip_serv_state_enhanced_sw", "OFF" },
            { "ul_voip_sch_opt_switch", "ON" },
            { "ul_vo_lte_data_size_est_switch", "OFF" }
        };
        
        public Service(IConfiguration configuration, IDateTimeProvider dateTimeProvider, IUserProvider userService)
        {
            var connectionString = configuration["RnocConnectionString"];
            _dbContext = new RnocDbContext(connectionString);
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }
        
        public async Task<R001DashboardResponse> GetDashboardDataAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            var configuredSites = await GetConfiguredSitesByDateAsync(date);
            var badConfigurations = await GetBadConfigurationsByDateAsync(date);
            var statistics = await GetStatisticsAsync(date);
            var parameterSummaries = await GetParameterSummariesAsync(date);
            
            return new R001DashboardResponse
            {
                Date = date,
                Statistics = statistics,
                ParameterSummaries = parameterSummaries,
                ConfiguredSites = configuredSites.Select(MapToConfiguredSite).ToList()
            };
        }
        
        public async Task<List<R001_DataRuntime>> GetConfiguredSitesByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            var sql = @"
                SELECT * FROM r001_data_runtime 
                WHERE report_date >= @startDate AND report_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''
                ORDER BY ne_name, cell_id";
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate);
            command.Parameters.AddWithValue("endDate", endDate);
            
            var results = new List<R001_DataRuntime>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapFromReader(reader));
            }
            
            return results;
        }
        
        public async Task<List<R001_DataRuntime>> GetConfiguredSitesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT * FROM r001_data_runtime 
                WHERE report_date >= @startDate AND report_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''
                ORDER BY ne_name, cell_id, report_date DESC";
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate.Date);
            command.Parameters.AddWithValue("endDate", endDate.Date.AddDays(1).AddSeconds(-1));
            
            var results = new List<R001_DataRuntime>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapFromReader(reader));
            }
            
            return results;
        }
        
        public async Task<List<R001_DataRuntimeBad>> GetBadConfigurationsByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            var sql = @"
                SELECT * FROM r001_data_runtime_bad 
                WHERE detected_date >= @startDate AND detected_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''
                ORDER BY ne_name, cell_id";
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate);
            command.Parameters.AddWithValue("endDate", endDate);
            
            var results = new List<R001_DataRuntimeBad>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapBadFromReader(reader));
            }
            
            return results;
        }
        
        public async Task<List<R001_DataRuntimeBad>> GetBadConfigurationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT * FROM r001_data_runtime_bad 
                WHERE detected_date >= @startDate AND detected_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''
                ORDER BY ne_name, cell_id, detected_date DESC";
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate.Date);
            command.Parameters.AddWithValue("endDate", endDate.Date.AddDays(1).AddSeconds(-1));
            
            var results = new List<R001_DataRuntimeBad>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapBadFromReader(reader));
            }
            
            return results;
        }
        
        // ⚡ Server-side pagination for configured sites
        public async Task<(List<R001_DataRuntime> Data, int TotalCount)> GetConfiguredSitesByDatePagedAsync(DateTime date, int page, int pageSize)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            // Get total count - ĐẾM DISTINCT (ne_name, cell_id)
            var countSql = @"
                SELECT COUNT(DISTINCT (ne_name, cell_id)) 
                FROM r001_data_runtime 
                WHERE report_date >= @startDate AND report_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''";
            
            int totalCount;
            using (var countCommand = new NpgsqlCommand(countSql, connection))
            {
                countCommand.Parameters.AddWithValue("startDate", startDate);
                countCommand.Parameters.AddWithValue("endDate", endDate);
                totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
            }
            
            // Get paged data - LẤY DISTINCT records
            var sql = @"
                SELECT DISTINCT ON (ne_name, cell_id) *
                FROM r001_data_runtime 
                WHERE report_date >= @startDate AND report_date <= @endDate
                    AND utran_ps_ho_switch IS NOT NULL 
                    AND utran_ps_ho_switch != ''
                ORDER BY ne_name, cell_id, report_date DESC
                LIMIT @pageSize OFFSET @offset";
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate);
            command.Parameters.AddWithValue("endDate", endDate);
            command.Parameters.AddWithValue("pageSize", pageSize);
            command.Parameters.AddWithValue("offset", (page - 1) * pageSize);
            
            var results = new List<R001_DataRuntime>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapFromReader(reader));
            }
            
            return (results, totalCount);
        }
        
        // ⚡ Get statistics: Total unique NE count
        public async Task<int> GetTotalUniqueNECountAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            var sql = @"
                SELECT COUNT(DISTINCT ne_name) 
                FROM r001_data_runtime 
                WHERE report_date >= @startDate AND report_date <= @endDate
                    AND ne_name IS NOT NULL 
                    AND ne_name != ''";
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate);
            command.Parameters.AddWithValue("endDate", endDate);
            
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
        
        // ⚡ Server-side pagination for bad configurations
        public async Task<(List<R001_DataRuntimeBad> Data, int TotalCount)> GetBadConfigurationsByDatePagedAsync(DateTime date, int page, int pageSize)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            // Get total count - CHỈ LẤY RECORDS CHƯA CÓ TRONG r001_scheduler_fix_parametter
            // CHỈ ĐẾM DISTINCT (ne_name, cell_id) - mỗi cặp chỉ tính 1 lần
            var countSql = @"
                SELECT COUNT(DISTINCT (b.ne_name, b.cell_id)) 
                FROM r001_data_runtime_bad b
                WHERE b.detected_date >= @startDate AND b.detected_date <= @endDate
                    AND b.utran_ps_ho_switch IS NOT NULL 
                    AND b.utran_ps_ho_switch != ''
                    AND NOT EXISTS (
                        SELECT 1 FROM r001_scheduler_fix_parametter f 
                        WHERE f.ne_name = b.ne_name 
                            AND f.cell_id = b.cell_id 
                            AND b.report_date >= f.report_date 
                            AND b.report_date < (f.report_date + INTERVAL '1 day')
                    )";
            
            int totalCount;
            using (var countCommand = new NpgsqlCommand(countSql, connection))
            {
                countCommand.Parameters.AddWithValue("startDate", startDate);
                countCommand.Parameters.AddWithValue("endDate", endDate);
                totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
            }
            
            // Get paged data - CHỈ LẤY RECORDS CHƯA CÓ TRONG r001_scheduler_fix_parametter
            // CHỈ LẤY 1 RECORD DUY NHẤT cho mỗi (ne_name, cell_id) - lấy record mới nhất
            var sql = @"
                SELECT DISTINCT ON (b.ne_name, b.cell_id) b.*
                FROM r001_data_runtime_bad b
                WHERE b.detected_date >= @startDate AND b.detected_date <= @endDate
                    AND b.utran_ps_ho_switch IS NOT NULL 
                    AND b.utran_ps_ho_switch != ''
                    AND NOT EXISTS (
                        SELECT 1 FROM r001_scheduler_fix_parametter f 
                        WHERE f.ne_name = b.ne_name 
                            AND f.cell_id = b.cell_id 
                            AND b.report_date >= f.report_date 
                            AND b.report_date < (f.report_date + INTERVAL '1 day')
                    )
                ORDER BY b.ne_name, b.cell_id, b.detected_date DESC
                LIMIT @pageSize OFFSET @offset";
            
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("startDate", startDate);
            command.Parameters.AddWithValue("endDate", endDate);
            command.Parameters.AddWithValue("pageSize", pageSize);
            command.Parameters.AddWithValue("offset", (page - 1) * pageSize);
            
            var results = new List<R001_DataRuntimeBad>();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(MapBadFromReader(reader));
            }
            
            return (results, totalCount);
        }
        
        public async Task<R001DetailResponse> GetCorrectConfigurationsAsync(R001DetailRequest request)
        {
            var configuredSites = await GetConfiguredSitesByDateAsync(request.Date ?? DateTime.Today);
            var correctSites = configuredSites.Where(site => IsConfigurationCorrect(site)).ToList();
            
            var totalCount = correctSites.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var pagedData = correctSites
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(site => new R001DetailItem
                {
                    NeName = site.NeName,
                    CellId = site.CellId,
                    ReportDate = site.ReportDate,
                    IsCorrect = true,
                    Parameters = GetParameterDetails(site)
                })
                .ToList();
            
            return new R001DetailResponse
            {
                Data = pagedData,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = request.Page
            };
        }
        
        public async Task<R001DetailResponse> GetIncorrectConfigurationsAsync(R001DetailRequest request)
        {
            var badConfigurations = await GetBadConfigurationsByDateAsync(request.Date ?? DateTime.Today);
            
            var totalCount = badConfigurations.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var pagedData = badConfigurations
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(bad => new R001DetailItem
                {
                    NeName = bad.NeName,
                    CellId = bad.CellId,
                    ReportDate = bad.DetectedDate,
                    IsCorrect = false,
                    Parameters = GetBadParameterDetails(bad)
                })
                .ToList();
            
            return new R001DetailResponse
            {
                Data = pagedData,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = request.Page
            };
        }
        
        public async Task<R001DetailResponse> GetParameterDetailsAsync(R001DetailRequest request)
        {
            if (request.IsCorrect == true)
            {
                return await GetCorrectConfigurationsAsync(request);
            }
            else
            {
                return await GetIncorrectConfigurationsAsync(request);
            }
        }
        
        public async Task<R001Statistics> GetStatisticsAsync(DateTime date)
        {
            var configuredSites = await GetConfiguredSitesByDateAsync(date);
            var badConfigurations = await GetBadConfigurationsByDateAsync(date);
            
            var totalConfigured = configuredSites.Count;
            var totalBad = badConfigurations.Count;
            var totalCorrect = totalConfigured - totalBad;
            
            return new R001Statistics
            {
                TotalConfiguredSites = totalConfigured,
                CorrectConfigurations = totalCorrect,
                IncorrectConfigurations = totalBad,
                CorrectPercentage = totalConfigured > 0 ? Math.Round((decimal)totalCorrect / totalConfigured * 100, 2) : 0
            };
        }
        
        public async Task<List<R001ParameterSummary>> GetParameterSummariesAsync(DateTime date)
        {
            var configuredSites = await GetConfiguredSitesByDateAsync(date);
            
            var parameterSummaries = new List<R001ParameterSummary>();
            
            // Standard values for each parameter
            var standardValues = new Dictionary<string, string>
            {
                { "utran_srvcc_switch", "ON" },
                { "utran_csfb_switch", "ON" },
                { "utran_flash_csfb_switch", "OFF" },
                { "geran_flash_csfb_switch", "OFF" },
                { "csfb_adaptive_blind_ho_switch", "OFF" },
                { "utran_csfb_steering_switch", "OFF" },
                { "idle_csfb_redirect_opt_switch", "ON" },
                { "dl_voip_bundling_switch", "ON" },
                { "ul_voip_pre_allocation_switch", "ON" },
                { "ul_voip_delay_sch_switch", "OFF" },
                { "ul_voip_load_based_sch_switch", "OFF" },
                { "ul_voip_serv_state_enhanced_sw", "OFF" },
                { "ul_voip_sch_opt_switch", "ON" },
                { "ul_vo_lte_data_size_est_switch", "OFF" }
            };
            
            foreach (var paramName in standardValues.Keys)
            {
                var totalCount = configuredSites.Count;
                var correctCount = 0;
                var incorrectCount = 0;
                
                // Count correct and incorrect for this specific parameter
                foreach (var site in configuredSites)
                {
                    var actualValue = GetParameterValue(site, paramName)?.ToUpper() ?? "";
                    var expectedValue = standardValues[paramName]?.ToUpper() ?? "";
                    
                    if (actualValue == expectedValue)
                    {
                        correctCount++;
                    }
                    else
                    {
                        incorrectCount++;
                    }
                }
                
                parameterSummaries.Add(new R001ParameterSummary
                {
                    ParameterName = paramName,
                    CorrectCount = correctCount,
                    IncorrectCount = incorrectCount,
                    TotalCount = totalCount,
                    CorrectPercentage = totalCount > 0 ? Math.Round((decimal)correctCount / totalCount * 100, 2) : 0
                });
            }
            
            return parameterSummaries;
        }
        
        public async Task<string> ExportConfiguredSitesToCsvAsync(DateTime startDate, DateTime endDate)
        {
            var sites = await GetConfiguredSitesByDateRangeAsync(startDate, endDate);
            
            var csvContent = new StringBuilder();
            csvContent.AppendLine("STT,NE Name,Cell ID,Report Date,UTRAN SRVCC Switch,UTRAN CSFB Switch,UTRAN Flash CSFB Switch,GERAN Flash CSFB Switch,CSFB Adaptive Blind HO Switch,UTRAN CSFB Steering Switch,Idle CSFB Redirect Opt Switch,DL VoIP Bundling Switch,UL VoIP Pre Allocation Switch,UL VoIP Delay SCH Switch,UL VoIP Load Based SCH Switch,UL VoIP Serv State Enhanced SW,UL VoIP SCH Opt Switch,UL VoLTE Data Size Est Switch");
            
            int stt = 1;
            foreach (var site in sites)
            {
                csvContent.AppendLine($"{stt}," +
                    $"\"{site.NeName}\"," +
                    $"{site.CellId}," +
                    $"\"{site.ReportDate?.ToString("dd/MM/yyyy HH:mm:ss")}\"," +
                    $"\"{site.UtranSrvccSwitch}\"," +
                    $"\"{site.UtranCsfbSwitch}\"," +
                    $"\"{site.UtranFlashCsfbSwitch}\"," +
                    $"\"{site.GeranFlashCsfbSwitch}\"," +
                    $"\"{site.CsfbAdaptiveBlindHoSwitch}\"," +
                    $"\"{site.UtranCsfbSteeringSwitch}\"," +
                    $"\"{site.IdleCsfbRedirectOptSwitch}\"," +
                    $"\"{site.DlVoipBundlingSwitch}\"," +
                    $"\"{site.UlVoipPreAllocationSwitch}\"," +
                    $"\"{site.UlVoipDelaySchSwitch}\"," +
                    $"\"{site.UlVoipLoadBasedSchSwitch}\"," +
                    $"\"{site.UlVoipServStateEnhancedSw}\"," +
                    $"\"{site.UlVoipSchOptSwitch}\"," +
                    $"\"{site.UlVoLteDataSizeEstSwitch}\"");
                stt++;
            }
            
            var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            return Convert.ToBase64String(csvBytes);
        }
        
        public async Task<string> ExportBadConfigurationsToCsvAsync(DateTime startDate, DateTime endDate)
        {
            var badConfigs = await GetBadConfigurationsByDateRangeAsync(startDate, endDate);
            
            var csvContent = new StringBuilder();
            csvContent.AppendLine("STT,NE Name,Cell ID,Detected Date,UTRAN SRVCC Switch,UTRAN CSFB Switch,UTRAN Flash CSFB Switch,GERAN Flash CSFB Switch,CSFB Adaptive Blind HO Switch,UTRAN CSFB Steering Switch,Idle CSFB Redirect Opt Switch,DL VoIP Bundling Switch,UL VoIP Pre Allocation Switch,UL VoIP Delay SCH Switch,UL VoIP Load Based SCH Switch,UL VoIP Serv State Enhanced SW,UL VoIP SCH Opt Switch,UL VoLTE Data Size Est Switch");
            
            int stt = 1;
            foreach (var bad in badConfigs)
            {
                csvContent.AppendLine($"{stt}," +
                    $"\"{bad.NeName}\"," +
                    $"{bad.CellId}," +
                    $"\"{bad.DetectedDate?.ToString("dd/MM/yyyy")}\"," +
                    $"\"{bad.UtranSrvccSwitch}\"," +
                    $"\"{bad.UtranCsfbSwitch}\"," +
                    $"\"{bad.UtranFlashCsfbSwitch}\"," +
                    $"\"{bad.GeranFlashCsfbSwitch}\"," +
                    $"\"{bad.CsfbAdaptiveBlindHoSwitch}\"," +
                    $"\"{bad.UtranCsfbSteeringSwitch}\"," +
                    $"\"{bad.IdleCsfbRedirectOptSwitch}\"," +
                    $"\"{bad.DlVoipBundlingSwitch}\"," +
                    $"\"{bad.UlVoipPreAllocationSwitch}\"," +
                    $"\"{bad.UlVoipDelaySchSwitch}\"," +
                    $"\"{bad.UlVoipLoadBasedSchSwitch}\"," +
                    $"\"{bad.UlVoipServStateEnhancedSw}\"," +
                    $"\"{bad.UlVoipSchOptSwitch}\"," +
                    $"\"{bad.UlVoLteDataSizeEstSwitch}\"");
                stt++;
            }
            
            var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());
            return Convert.ToBase64String(csvBytes);
        }
        
        #region Private Helper Methods
        
        private R001_DataRuntime MapFromReader(NpgsqlDataReader reader)
        {
            return new R001_DataRuntime
            {
                Id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                NeName = reader.IsDBNull("ne_name") ? null : reader.GetString("ne_name"),
                CellId = reader.IsDBNull("cell_id") ? 0 : reader.GetInt32("cell_id"),
                UtranPsHoSwitch = reader.IsDBNull("utran_ps_ho_switch") ? null : reader.GetString("utran_ps_ho_switch"),
                ReportDate = reader.IsDBNull("report_date") ? null : reader.GetDateTime("report_date"),
                CreatedAt = reader.IsDBNull("created_at") ? null : reader.GetDateTime("created_at"),
                UpdatedAt = reader.IsDBNull("updated_at") ? null : reader.GetDateTime("updated_at"),
                UtranSrvccSwitch = reader.IsDBNull("utran_srvcc_switch") ? null : reader.GetString("utran_srvcc_switch"),
                UtranCsfbSwitch = reader.IsDBNull("utran_csfb_switch") ? null : reader.GetString("utran_csfb_switch"),
                UtranFlashCsfbSwitch = reader.IsDBNull("utran_flash_csfb_switch") ? null : reader.GetString("utran_flash_csfb_switch"),
                GeranFlashCsfbSwitch = reader.IsDBNull("geran_flash_csfb_switch") ? null : reader.GetString("geran_flash_csfb_switch"),
                CsfbAdaptiveBlindHoSwitch = reader.IsDBNull("csfb_adaptive_blind_ho_switch") ? null : reader.GetString("csfb_adaptive_blind_ho_switch"),
                UtranCsfbSteeringSwitch = reader.IsDBNull("utran_csfb_steering_switch") ? null : reader.GetString("utran_csfb_steering_switch"),
                IdleCsfbRedirectOptSwitch = reader.IsDBNull("idle_csfb_redirect_opt_switch") ? null : reader.GetString("idle_csfb_redirect_opt_switch"),
                DlVoipBundlingSwitch = reader.IsDBNull("dl_voip_bundling_switch") ? null : reader.GetString("dl_voip_bundling_switch"),
                UlVoipPreAllocationSwitch = reader.IsDBNull("ul_voip_pre_allocation_switch") ? null : reader.GetString("ul_voip_pre_allocation_switch"),
                UlVoipDelaySchSwitch = reader.IsDBNull("ul_voip_delay_sch_switch") ? null : reader.GetString("ul_voip_delay_sch_switch"),
                UlVoipLoadBasedSchSwitch = reader.IsDBNull("ul_voip_load_based_sch_switch") ? null : reader.GetString("ul_voip_load_based_sch_switch"),
                UlVoipServStateEnhancedSw = reader.IsDBNull("ul_voip_serv_state_enhanced_sw") ? null : reader.GetString("ul_voip_serv_state_enhanced_sw"),
                UlVoipSchOptSwitch = reader.IsDBNull("ul_voip_sch_opt_switch") ? null : reader.GetString("ul_voip_sch_opt_switch"),
                UlVoLteDataSizeEstSwitch = reader.IsDBNull("ul_vo_lte_data_size_est_switch") ? null : reader.GetString("ul_vo_lte_data_size_est_switch")
            };
        }
        
        private R001_DataRuntimeBad MapBadFromReader(NpgsqlDataReader reader)
        {
            return new R001_DataRuntimeBad
            {
                Id = reader.GetInt32("id"),
                NeName = reader.IsDBNull("ne_name") ? null : reader.GetString("ne_name"),
                CellId = reader.GetInt32("cell_id"),
                UtranPsHoSwitch = reader.IsDBNull("utran_ps_ho_switch") ? null : reader.GetString("utran_ps_ho_switch"),
                ReportDate = reader.IsDBNull("report_date") ? null : reader.GetDateTime("report_date"),
                CreatedAt = reader.IsDBNull("created_at") ? null : reader.GetDateTime("created_at"),
                UpdatedAt = reader.IsDBNull("updated_at") ? null : reader.GetDateTime("updated_at"),
                UtranSrvccSwitch = reader.IsDBNull("utran_srvcc_switch") ? null : reader.GetString("utran_srvcc_switch"),
                UtranCsfbSwitch = reader.IsDBNull("utran_csfb_switch") ? null : reader.GetString("utran_csfb_switch"),
                UtranFlashCsfbSwitch = reader.IsDBNull("utran_flash_csfb_switch") ? null : reader.GetString("utran_flash_csfb_switch"),
                GeranFlashCsfbSwitch = reader.IsDBNull("geran_flash_csfb_switch") ? null : reader.GetString("geran_flash_csfb_switch"),
                CsfbAdaptiveBlindHoSwitch = reader.IsDBNull("csfb_adaptive_blind_ho_switch") ? null : reader.GetString("csfb_adaptive_blind_ho_switch"),
                UtranCsfbSteeringSwitch = reader.IsDBNull("utran_csfb_steering_switch") ? null : reader.GetString("utran_csfb_steering_switch"),
                IdleCsfbRedirectOptSwitch = reader.IsDBNull("idle_csfb_redirect_opt_switch") ? null : reader.GetString("idle_csfb_redirect_opt_switch"),
                DlVoipBundlingSwitch = reader.IsDBNull("dl_voip_bundling_switch") ? null : reader.GetString("dl_voip_bundling_switch"),
                UlVoipPreAllocationSwitch = reader.IsDBNull("ul_voip_pre_allocation_switch") ? null : reader.GetString("ul_voip_pre_allocation_switch"),
                UlVoipDelaySchSwitch = reader.IsDBNull("ul_voip_delay_sch_switch") ? null : reader.GetString("ul_voip_delay_sch_switch"),
                UlVoipLoadBasedSchSwitch = reader.IsDBNull("ul_voip_load_based_sch_switch") ? null : reader.GetString("ul_voip_load_based_sch_switch"),
                UlVoipServStateEnhancedSw = reader.IsDBNull("ul_voip_serv_state_enhanced_sw") ? null : reader.GetString("ul_voip_serv_state_enhanced_sw"),
                UlVoipSchOptSwitch = reader.IsDBNull("ul_voip_sch_opt_switch") ? null : reader.GetString("ul_voip_sch_opt_switch"),
                UlVoLteDataSizeEstSwitch = reader.IsDBNull("ul_vo_lte_data_size_est_switch") ? null : reader.GetString("ul_vo_lte_data_size_est_switch"),
                DetectedDate = reader.IsDBNull("detected_date") ? null : reader.GetDateTime("detected_date")
            };
        }
        
        private R001ConfiguredSite MapToConfiguredSite(R001_DataRuntime data)
        {
            return new R001ConfiguredSite
            {
                NeName = data.NeName,
                CellId = data.CellId,
                ReportDate = data.ReportDate,
                CreatedAt = data.CreatedAt,
                IsCorrect = IsConfigurationCorrect(data),
                Parameters = GetParameterDetails(data)
            };
        }
        
        private bool IsConfigurationCorrect(R001_DataRuntime data)
        {
            var parameters = GetParameterActualValues(data);
            return parameters.All(kvp => kvp.Value == _standardValues.GetValueOrDefault(kvp.Key, ""));
        }
        
        private List<R001ParameterDetail> GetParameterDetails(R001_DataRuntime data)
        {
            var actualValues = GetParameterActualValues(data);
            return actualValues.Select(kvp => new R001ParameterDetail
            {
                ParameterName = kvp.Key,
                ActualValue = kvp.Value,
                ExpectedValue = _standardValues.GetValueOrDefault(kvp.Key, ""),
                IsCorrect = kvp.Value == _standardValues.GetValueOrDefault(kvp.Key, "")
            }).ToList();
        }
        
        private List<R001ParameterDetail> GetBadParameterDetails(R001_DataRuntimeBad data)
        {
            var actualValues = GetBadParameterActualValues(data);
            return actualValues.Select(kvp => new R001ParameterDetail
            {
                ParameterName = kvp.Key,
                ActualValue = kvp.Value,
                ExpectedValue = _standardValues.GetValueOrDefault(kvp.Key, ""),
                IsCorrect = false // All bad configurations are incorrect
            }).ToList();
        }
        
        private Dictionary<string, string> GetParameterActualValues(R001_DataRuntime data)
        {
            return new Dictionary<string, string>
            {
                { "utran_srvcc_switch", data.UtranSrvccSwitch ?? "" },
                { "utran_csfb_switch", data.UtranCsfbSwitch ?? "" },
                { "utran_flash_csfb_switch", data.UtranFlashCsfbSwitch ?? "" },
                { "geran_flash_csfb_switch", data.GeranFlashCsfbSwitch ?? "" },
                { "csfb_adaptive_blind_ho_switch", data.CsfbAdaptiveBlindHoSwitch ?? "" },
                { "utran_csfb_steering_switch", data.UtranCsfbSteeringSwitch ?? "" },
                { "idle_csfb_redirect_opt_switch", data.IdleCsfbRedirectOptSwitch ?? "" },
                { "dl_voip_bundling_switch", data.DlVoipBundlingSwitch ?? "" },
                { "ul_voip_pre_allocation_switch", data.UlVoipPreAllocationSwitch ?? "" },
                { "ul_voip_delay_sch_switch", data.UlVoipDelaySchSwitch ?? "" },
                { "ul_voip_load_based_sch_switch", data.UlVoipLoadBasedSchSwitch ?? "" },
                { "ul_voip_serv_state_enhanced_sw", data.UlVoipServStateEnhancedSw ?? "" },
                { "ul_voip_sch_opt_switch", data.UlVoipSchOptSwitch ?? "" },
                { "ul_vo_lte_data_size_est_switch", data.UlVoLteDataSizeEstSwitch ?? "" }
            };
        }
        
        private string GetParameterValue(R001_DataRuntime data, string parameterName)
        {
            switch (parameterName)
            {
                case "utran_srvcc_switch": return data.UtranSrvccSwitch ?? "";
                case "utran_csfb_switch": return data.UtranCsfbSwitch ?? "";
                case "utran_flash_csfb_switch": return data.UtranFlashCsfbSwitch ?? "";
                case "geran_flash_csfb_switch": return data.GeranFlashCsfbSwitch ?? "";
                case "csfb_adaptive_blind_ho_switch": return data.CsfbAdaptiveBlindHoSwitch ?? "";
                case "utran_csfb_steering_switch": return data.UtranCsfbSteeringSwitch ?? "";
                case "idle_csfb_redirect_opt_switch": return data.IdleCsfbRedirectOptSwitch ?? "";
                case "dl_voip_bundling_switch": return data.DlVoipBundlingSwitch ?? "";
                case "ul_voip_pre_allocation_switch": return data.UlVoipPreAllocationSwitch ?? "";
                case "ul_voip_delay_sch_switch": return data.UlVoipDelaySchSwitch ?? "";
                case "ul_voip_load_based_sch_switch": return data.UlVoipLoadBasedSchSwitch ?? "";
                case "ul_voip_serv_state_enhanced_sw": return data.UlVoipServStateEnhancedSw ?? "";
                case "ul_voip_sch_opt_switch": return data.UlVoipSchOptSwitch ?? "";
                case "ul_vo_lte_data_size_est_switch": return data.UlVoLteDataSizeEstSwitch ?? "";
                default: return "";
            }
        }
        
        private Dictionary<string, string> GetBadParameterActualValues(R001_DataRuntimeBad data)
        {
            return new Dictionary<string, string>
            {
                { "utran_srvcc_switch", data.UtranSrvccSwitch ?? "" },
                { "utran_csfb_switch", data.UtranCsfbSwitch ?? "" },
                { "utran_flash_csfb_switch", data.UtranFlashCsfbSwitch ?? "" },
                { "geran_flash_csfb_switch", data.GeranFlashCsfbSwitch ?? "" },
                { "csfb_adaptive_blind_ho_switch", data.CsfbAdaptiveBlindHoSwitch ?? "" },
                { "utran_csfb_steering_switch", data.UtranCsfbSteeringSwitch ?? "" },
                { "idle_csfb_redirect_opt_switch", data.IdleCsfbRedirectOptSwitch ?? "" },
                { "dl_voip_bundling_switch", data.DlVoipBundlingSwitch ?? "" },
                { "ul_voip_pre_allocation_switch", data.UlVoipPreAllocationSwitch ?? "" },
                { "ul_voip_delay_sch_switch", data.UlVoipDelaySchSwitch ?? "" },
                { "ul_voip_load_based_sch_switch", data.UlVoipLoadBasedSchSwitch ?? "" },
                { "ul_voip_serv_state_enhanced_sw", data.UlVoipServStateEnhancedSw ?? "" },
                { "ul_voip_sch_opt_switch", data.UlVoipSchOptSwitch ?? "" },
                { "ul_vo_lte_data_size_est_switch", data.UlVoLteDataSizeEstSwitch ?? "" }
            };
        }
        
        private int CountBadParameterValue(List<R001_DataRuntimeBad> badConfigs, string parameterName)
        {
            return badConfigs.Count(bad =>
            {
                var actualValues = GetBadParameterActualValues(bad);
                var actualValue = actualValues.GetValueOrDefault(parameterName, "");
                var expectedValue = _standardValues.GetValueOrDefault(parameterName, "");
                return actualValue != expectedValue;
            });
        }
        
        #endregion
        
        #region Fix Configuration Methods
        
        public async Task<List<R001_SchedulerFixParameter>> FixSingleConfigurationAsync(R001_SchedulerFixParameter fixParameter)
        {
            // Generate separate command/baseline pairs
            var commandPairs = GenerateCommandPairs(fixParameter);
            
            // If no commands generated, return empty list
            if (commandPairs.Count == 0)
            {
                return new List<R001_SchedulerFixParameter>();
            }
            
            var insertedRecords = new List<R001_SchedulerFixParameter>();
            
            using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync();
            
            // Delete existing records with same ne_name + cell_id + baseline_type to avoid duplicates
            var baselineTypes = commandPairs.Select(cp => cp.BaselineType).Distinct().ToList();
            var deleteParams = string.Join(",", baselineTypes.Select((_, i) => $"@baselineType{i}"));
            
            var deleteSql = $@"
                DELETE FROM r001_scheduler_fix_parametter 
                WHERE ne_name = @neName 
                  AND cell_id = @cellId 
                  AND baseline_type IN ({deleteParams})";
            
            using (var deleteCmd = new NpgsqlCommand(deleteSql, connection))
            {
                deleteCmd.Parameters.AddWithValue("neName", fixParameter.NeName ?? (object)DBNull.Value);
                deleteCmd.Parameters.AddWithValue("cellId", fixParameter.CellId);
                for (int i = 0; i < baselineTypes.Count; i++)
                {
                    deleteCmd.Parameters.AddWithValue($"baselineType{i}", baselineTypes[i]);
                }
                await deleteCmd.ExecuteNonQueryAsync();
            }
            
                var sql = @"
                    INSERT INTO r001_scheduler_fix_parametter 
                    (id, ne_name, cell_id, utran_ps_ho_switch, report_date, 
                     createddatetime, createdby, updateddatetime, updatedby,
                     utran_srvcc_switch, utran_csfb_switch, utran_flash_csfb_switch, geran_flash_csfb_switch,
                     csfb_adaptive_blind_ho_switch, utran_csfb_steering_switch, idle_csfb_redirect_opt_switch,
                     dl_voip_bundling_switch, ul_voip_pre_allocation_switch, ul_voip_delay_sch_switch,
                     ul_voip_load_based_sch_switch, ul_voip_serv_state_enhanced_sw, ul_voip_sch_opt_switch,
                     ul_vo_lte_data_size_est_switch, updated_edit, cmd_at, cmd_reg_at, baseline_type, status)
                    VALUES (@id, @neName, @cellId, @utranPsHoSwitch, @reportDate,
                            @createdDateTime, @createdBy, @updatedDateTime, @updatedBy,
                            @utranSrvccSwitch, @utranCsfbSwitch, @utranFlashCsfbSwitch, @geranFlashCsfbSwitch,
                            @csfbAdaptiveBlindHoSwitch, @utranCsfbSteeringSwitch, @idleCsfbRedirectOptSwitch,
                            @dlVoipBundlingSwitch, @ulVoipPreAllocationSwitch, @ulVoipDelaySchSwitch,
                            @ulVoipLoadBasedSchSwitch, @ulVoipServStateEnhancedSw, @ulVoipSchOptSwitch,
                            @ulVoLteDataSizeEstSwitch, @updatedEdit, @cmdAt, @cmdRegAt, @baselineType, @status)";
            
            // Insert one record per baseline type
            foreach (var (command, baselineType) in commandPairs)
            {
                // Create a new record for this baseline type
                var record = new R001_SchedulerFixParameter
                {
                    Id = Guid.NewGuid(), // Unique ID for each record
                    NeName = fixParameter.NeName,
                    CellId = fixParameter.CellId,
                    UtranPsHoSwitch = fixParameter.UtranPsHoSwitch,
                    ReportDate = fixParameter.ReportDate,
                    CreatedDateTime = DateTimeOffset.Now,
                    UpdatedDateTime = DateTimeOffset.Now,
                    UpdatedEdit = DateTime.Now,
                    UtranSrvccSwitch = fixParameter.UtranSrvccSwitch,
                    UtranCsfbSwitch = fixParameter.UtranCsfbSwitch,
                    UtranFlashCsfbSwitch = fixParameter.UtranFlashCsfbSwitch,
                    GeranFlashCsfbSwitch = fixParameter.GeranFlashCsfbSwitch,
                    CsfbAdaptiveBlindHoSwitch = fixParameter.CsfbAdaptiveBlindHoSwitch,
                    UtranCsfbSteeringSwitch = fixParameter.UtranCsfbSteeringSwitch,
                    IdleCsfbRedirectOptSwitch = fixParameter.IdleCsfbRedirectOptSwitch,
                    DlVoipBundlingSwitch = fixParameter.DlVoipBundlingSwitch,
                    UlVoipPreAllocationSwitch = fixParameter.UlVoipPreAllocationSwitch,
                    UlVoipDelaySchSwitch = fixParameter.UlVoipDelaySchSwitch,
                    UlVoipLoadBasedSchSwitch = fixParameter.UlVoipLoadBasedSchSwitch,
                    UlVoipServStateEnhancedSw = fixParameter.UlVoipServStateEnhancedSw,
                    UlVoipSchOptSwitch = fixParameter.UlVoipSchOptSwitch,
                    UlVoLteDataSizeEstSwitch = fixParameter.UlVoLteDataSizeEstSwitch,
                    CmdAt = command, // Specific command for this baseline type
                    CmdRegAt = $"REG NE:NAME=\"{fixParameter.NeName}\";", // REG command to run before cmd_at
                    BaselineType = baselineType, // Specific baseline type (SRVCC, CSFB, or VOLTE)
                    Status = 0 // Default status: 0 = Pending
                };
                
                // Set user info if available
                if (_userProvider != null && _userProvider.IsAuthenticated)
                {
                    record.CreatedBy = _userProvider.UserName;
                    record.UpdatedBy = _userProvider.UserName;
                }
                
                using var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("id", record.Id);
                cmd.Parameters.AddWithValue("neName", record.NeName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("cellId", record.CellId);
                cmd.Parameters.AddWithValue("utranPsHoSwitch", record.UtranPsHoSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("reportDate", record.ReportDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("createdDateTime", record.CreatedDateTime);
                cmd.Parameters.AddWithValue("createdBy", record.CreatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("updatedDateTime", record.UpdatedDateTime ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("updatedBy", record.UpdatedBy ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("utranSrvccSwitch", record.UtranSrvccSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("utranCsfbSwitch", record.UtranCsfbSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("utranFlashCsfbSwitch", record.UtranFlashCsfbSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("geranFlashCsfbSwitch", record.GeranFlashCsfbSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("csfbAdaptiveBlindHoSwitch", record.CsfbAdaptiveBlindHoSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("utranCsfbSteeringSwitch", record.UtranCsfbSteeringSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("idleCsfbRedirectOptSwitch", record.IdleCsfbRedirectOptSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("dlVoipBundlingSwitch", record.DlVoipBundlingSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoipPreAllocationSwitch", record.UlVoipPreAllocationSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoipDelaySchSwitch", record.UlVoipDelaySchSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoipLoadBasedSchSwitch", record.UlVoipLoadBasedSchSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoipServStateEnhancedSw", record.UlVoipServStateEnhancedSw ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoipSchOptSwitch", record.UlVoipSchOptSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ulVoLteDataSizeEstSwitch", record.UlVoLteDataSizeEstSwitch ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("updatedEdit", record.UpdatedEdit.Value);
                cmd.Parameters.AddWithValue("cmdAt", record.CmdAt);
                cmd.Parameters.AddWithValue("cmdRegAt", record.CmdRegAt);
                cmd.Parameters.AddWithValue("baselineType", record.BaselineType);
                cmd.Parameters.AddWithValue("status", record.Status ?? (object)DBNull.Value);
                
                await cmd.ExecuteNonQueryAsync();
                insertedRecords.Add(record);
            }
            
            return insertedRecords;
        }
        
        public async Task<List<R001_SchedulerFixParameter>> FixAllConfigurationsAsync(List<R001_SchedulerFixParameter> fixRequests)
        {
            var fixedList = new List<R001_SchedulerFixParameter>();
            
            foreach (var fixRequest in fixRequests)
            {
                // FixSingleConfigurationAsync now returns List<R001_SchedulerFixParameter>
                var fixedItems = await FixSingleConfigurationAsync(fixRequest);
                fixedList.AddRange(fixedItems); // Add all baseline type records
            }
            
            return fixedList;
        }
        
        // Generate list of separate command/baseline pairs (one per baseline type)
        private List<(string Command, string BaselineType)> GenerateCommandPairs(R001_SchedulerFixParameter fixParam)
        {
            var commandPairs = new List<(string, string)>();
            
            // Check SRVCC parameters
            if (fixParam.UtranSrvccSwitch?.ToUpper() != _standardValues["utran_srvcc_switch"])
            {
                var cmd = $"MOD CELLHOPARACFG:LOCALCELLID={fixParam.CellId},HOMODESWITCH=UtranSrvccSwitch-{(_standardValues["utran_srvcc_switch"] == "ON" ? "1" : "0")}; {{{fixParam.NeName}}}";
                commandPairs.Add((cmd, "SRVCC"));
            }
            
            // Check CSFB parameters
            var csfbNeedsUpdate = false;
            var csfbParts = new List<string>();
            
            if (fixParam.UtranCsfbSwitch?.ToUpper() != _standardValues["utran_csfb_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"UtranCsfbSwitch-{(_standardValues["utran_csfb_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.GeranFlashCsfbSwitch?.ToUpper() != _standardValues["geran_flash_csfb_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"GeranCsfbSwitch-{(_standardValues["geran_flash_csfb_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UtranFlashCsfbSwitch?.ToUpper() != _standardValues["utran_flash_csfb_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"UtranFlashCsfbSwitch-{(_standardValues["utran_flash_csfb_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.CsfbAdaptiveBlindHoSwitch?.ToUpper() != _standardValues["csfb_adaptive_blind_ho_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"CsfbAdaptiveBlindHoSwitch-{(_standardValues["csfb_adaptive_blind_ho_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UtranCsfbSteeringSwitch?.ToUpper() != _standardValues["utran_csfb_steering_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"UtranCsfbSteeringSwitch-{(_standardValues["utran_csfb_steering_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.IdleCsfbRedirectOptSwitch?.ToUpper() != _standardValues["idle_csfb_redirect_opt_switch"])
            {
                csfbNeedsUpdate = true;
                csfbParts.Add($"IdleCsfbRedirectOptSwitch-{(_standardValues["idle_csfb_redirect_opt_switch"] == "ON" ? "1" : "0")}");
            }
            
            if (csfbNeedsUpdate)
            {
                var cmd = $"MOD CELLALGOSWITCH:LOCALCELLID={fixParam.CellId},HOALLOWEDSWITCH={string.Join("&", csfbParts)}; {{{fixParam.NeName}}}";
                commandPairs.Add((cmd, "CSFB"));
            }
            
            // Check VOLTE parameters
            var volteNeedsUpdate = false;
            var volteParts = new List<string>();
            
            if (fixParam.UlVoipPreAllocationSwitch?.ToUpper() != _standardValues["ul_voip_pre_allocation_switch"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoipPreAllocationSwitch-{(_standardValues["ul_voip_pre_allocation_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UlVoipDelaySchSwitch?.ToUpper() != _standardValues["ul_voip_delay_sch_switch"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoipDelaySchSwitch-{(_standardValues["ul_voip_delay_sch_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UlVoipLoadBasedSchSwitch?.ToUpper() != _standardValues["ul_voip_load_based_sch_switch"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoIPLoadBasedSchSwitch-{(_standardValues["ul_voip_load_based_sch_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UlVoipSchOptSwitch?.ToUpper() != _standardValues["ul_voip_sch_opt_switch"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoipSchOptSwitch-{(_standardValues["ul_voip_sch_opt_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UlVoLteDataSizeEstSwitch?.ToUpper() != _standardValues["ul_vo_lte_data_size_est_switch"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoLTEDataSizeEstSwitch-{(_standardValues["ul_vo_lte_data_size_est_switch"] == "ON" ? "1" : "0")}");
            }
            if (fixParam.UlVoipServStateEnhancedSw?.ToUpper() != _standardValues["ul_voip_serv_state_enhanced_sw"])
            {
                volteNeedsUpdate = true;
                volteParts.Add($"UlVoipServStateEnhancedSw-{(_standardValues["ul_voip_serv_state_enhanced_sw"] == "ON" ? "1" : "0")}");
            }
            
            if (volteNeedsUpdate)
            {
                var cmd = $"MOD CELLULSCHALGO:LOCALCELLID={fixParam.CellId},ULENHENCEDVOIPSCHSW={string.Join("&", volteParts)}; {{{fixParam.NeName}}}";
                commandPairs.Add((cmd, "VOLTE"));
            }
            
            return commandPairs;
        }
        
        #endregion
        
        #region Get Fix Parameters
        
        public async Task<(List<R001_SchedulerFixParameter> Data, int TotalCount)> GetFixParametersByDatePagedAsync(DateTime date, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            
            using (var conn = new NpgsqlConnection(_dbContext.Database.GetConnectionString()))
            {
                await conn.OpenAsync();
                
                // Get total count
                var countQuery = @"
                    SELECT COUNT(*) 
                    FROM r001_scheduler_fix_parametter 
                    WHERE report_date = @Date";
                    
                var totalCount = 0;
                using (var countCmd = new NpgsqlCommand(countQuery, conn))
                {
                    countCmd.Parameters.AddWithValue("@Date", date.Date);
                    totalCount = Convert.ToInt32(await countCmd.ExecuteScalarAsync());
                }
                
                // Get paginated data
                var query = @"
                    SELECT 
                        id,
                        ne_name,
                        cell_id,
                        utran_ps_ho_switch,
                        report_date,
                        utran_srvcc_switch,
                        utran_csfb_switch,
                        utran_flash_csfb_switch,
                        geran_flash_csfb_switch,
                        csfb_adaptive_blind_ho_switch,
                        utran_csfb_steering_switch,
                        idle_csfb_redirect_opt_switch,
                        dl_voip_bundling_switch,
                        ul_voip_pre_allocation_switch,
                        ul_voip_delay_sch_switch,
                        ul_voip_load_based_sch_switch,
                        ul_voip_serv_state_enhanced_sw,
                        ul_voip_sch_opt_switch,
                        ul_vo_lte_data_size_est_switch,
                        updated_edit,
                        cmd_at,
                        cmd_reg_at,
                        baseline_type,
                        status,
                        created_date_time,
                        created_by,
                        updated_date_time,
                        updated_by
                    FROM r001_scheduler_fix_parametter 
                    WHERE report_date = @Date
                    ORDER BY created_date_time DESC, ne_name, cell_id
                    LIMIT @PageSize OFFSET @Skip";
                    
                var result = new List<R001_SchedulerFixParameter>();
                
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@Skip", skip);
                    
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new R001_SchedulerFixParameter
                            {
                                Id = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                                NeName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                CellId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                UtranPsHoSwitch = reader.IsDBNull(3) ? null : reader.GetString(3),
                                ReportDate = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                                UtranSrvccSwitch = reader.IsDBNull(5) ? null : reader.GetString(5),
                                UtranCsfbSwitch = reader.IsDBNull(6) ? null : reader.GetString(6),
                                UtranFlashCsfbSwitch = reader.IsDBNull(7) ? null : reader.GetString(7),
                                GeranFlashCsfbSwitch = reader.IsDBNull(8) ? null : reader.GetString(8),
                                CsfbAdaptiveBlindHoSwitch = reader.IsDBNull(9) ? null : reader.GetString(9),
                                UtranCsfbSteeringSwitch = reader.IsDBNull(10) ? null : reader.GetString(10),
                                IdleCsfbRedirectOptSwitch = reader.IsDBNull(11) ? null : reader.GetString(11),
                                DlVoipBundlingSwitch = reader.IsDBNull(12) ? null : reader.GetString(12),
                                UlVoipPreAllocationSwitch = reader.IsDBNull(13) ? null : reader.GetString(13),
                                UlVoipDelaySchSwitch = reader.IsDBNull(14) ? null : reader.GetString(14),
                                UlVoipLoadBasedSchSwitch = reader.IsDBNull(15) ? null : reader.GetString(15),
                                UlVoipServStateEnhancedSw = reader.IsDBNull(16) ? null : reader.GetString(16),
                                UlVoipSchOptSwitch = reader.IsDBNull(17) ? null : reader.GetString(17),
                                UlVoLteDataSizeEstSwitch = reader.IsDBNull(18) ? null : reader.GetString(18),
                                UpdatedEdit = reader.IsDBNull(19) ? null : (DateTime?)reader.GetDateTime(19),
                                CmdAt = reader.IsDBNull(20) ? null : reader.GetString(20),
                                CmdRegAt = reader.IsDBNull(21) ? null : reader.GetString(21),
                                BaselineType = reader.IsDBNull(22) ? null : reader.GetString(22),
                                Status = reader.IsDBNull(23) ? null : (int?)reader.GetInt32(23),
                                CreatedDateTime = reader.IsDBNull(24) ? DateTime.MinValue : reader.GetDateTime(24),
                                CreatedBy = reader.IsDBNull(25) ? null : reader.GetString(25),
                                UpdatedDateTime = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                UpdatedBy = reader.IsDBNull(27) ? null : reader.GetString(27)
                            });
                        }
                    }
                }
                
                return (result, totalCount);
            }
        }
        
        #endregion
    }
}