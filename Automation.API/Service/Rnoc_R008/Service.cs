using Network.API.Infrastructure;
using Network.API.Model;
using Network.API.ViewModel.Dashboard;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.API.Service.Rnoc_R008
{
    public class Service : IService
    {
        private readonly ILogger<Service> _logger;
        private readonly string _connectionString;
        
        public Service(IConfiguration configuration, ILogger<Service> logger)
        {
            _logger = logger;
            _connectionString = configuration.GetValue<string>("RnocConnectionString");
        }
        
        // Get dashboard statistics by day (hourly filter within a day)
        public async Task<R008DashboardResponse> GetDashboardByDayAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var sql = @"
                        SELECT 
                            COUNT(DISTINCT CONCAT(srn, '-', cn, '-', sn, '-', localcellid)) as TotalCells,
                            COUNT(DISTINCT CASE WHEN run_off = 1 OR run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecuted,
                            COUNT(DISTINCT CASE WHEN (run_off IS NULL OR run_off = 0) AND (run_on IS NULL OR run_on = 0) THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsNotExecuted,
                            COUNT(DISTINCT CASE WHEN run_off = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOff,
                            COUNT(DISTINCT CASE WHEN run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOn,
                            COALESCE(SUM(EXTRACT(EPOCH FROM (time_run_on - time_run_off)) / 3600), 0) as TotalExecutionHours
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                            AND time_run_off IS NOT NULL 
                            AND time_run_on IS NOT NULL";
                    
                    var result = await connection.QueryFirstOrDefaultAsync<R008DashboardResponse>(
                        sql, 
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    return result ?? new R008DashboardResponse();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDashboardByDayAsync: {ex.Message}");
                throw;
            }
        }
        
        // Get dashboard statistics by week
        public async Task<R008DashboardResponse> GetDashboardByWeekAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var sql = @"
                        SELECT 
                            COUNT(DISTINCT CONCAT(srn, '-', cn, '-', sn, '-', localcellid)) as TotalCells,
                            COUNT(DISTINCT CASE WHEN run_off = 1 OR run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecuted,
                            COUNT(DISTINCT CASE WHEN (run_off IS NULL OR run_off = 0) AND (run_on IS NULL OR run_on = 0) THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsNotExecuted,
                            COUNT(DISTINCT CASE WHEN run_off = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOff,
                            COUNT(DISTINCT CASE WHEN run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOn,
                            COALESCE(SUM(EXTRACT(EPOCH FROM (time_run_on - time_run_off)) / 3600), 0) as TotalExecutionHours
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                            AND time_run_off IS NOT NULL 
                            AND time_run_on IS NOT NULL";
                    
                    var result = await connection.QueryFirstOrDefaultAsync<R008DashboardResponse>(
                        sql, 
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    // Get daily breakdown for the week
                    var dailySql = @"
                        SELECT 
                            DATE(time) as Date,
                            COUNT(DISTINCT CONCAT(srn, '-', cn, '-', sn, '-', localcellid)) as TotalCells,
                            COUNT(DISTINCT CASE WHEN run_off = 1 OR run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecuted,
                            COUNT(DISTINCT CASE WHEN (run_off IS NULL OR run_off = 0) AND (run_on IS NULL OR run_on = 0) THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsNotExecuted,
                            COUNT(DISTINCT CASE WHEN run_off = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOff,
                            COUNT(DISTINCT CASE WHEN run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOn,
                            COALESCE(SUM(EXTRACT(EPOCH FROM (time_run_on - time_run_off)) / 3600), 0) as TotalExecutionHours
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                            AND time_run_off IS NOT NULL 
                            AND time_run_on IS NOT NULL
                        GROUP BY DATE(time)
                        ORDER BY DATE(time)";
                    
                    var dailyStats = await connection.QueryAsync<R008_DailyStatistics>(
                        dailySql,
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    result = result ?? new R008DashboardResponse();
                    result.DailyStatistics = dailyStats.ToList();
                    
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDashboardByWeekAsync: {ex.Message}");
                throw;
            }
        }
        
        // Get dashboard statistics by month
        public async Task<R008DashboardResponse> GetDashboardByMonthAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var sql = @"
                        SELECT 
                            COUNT(DISTINCT CONCAT(srn, '-', cn, '-', sn, '-', localcellid)) as TotalCells,
                            COUNT(DISTINCT CASE WHEN run_off = 1 OR run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecuted,
                            COUNT(DISTINCT CASE WHEN (run_off IS NULL OR run_off = 0) AND (run_on IS NULL OR run_on = 0) THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsNotExecuted,
                            COUNT(DISTINCT CASE WHEN run_off = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOff,
                            COUNT(DISTINCT CASE WHEN run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOn,
                            COALESCE(SUM(EXTRACT(EPOCH FROM (time_run_on - time_run_off)) / 3600), 0) as TotalExecutionHours
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                            AND time_run_off IS NOT NULL 
                            AND time_run_on IS NOT NULL";
                    
                    var result = await connection.QueryFirstOrDefaultAsync<R008DashboardResponse>(
                        sql, 
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    // Get daily breakdown for the month
                    var dailySql = @"
                        SELECT 
                            DATE(time) as Date,
                            COUNT(DISTINCT CONCAT(srn, '-', cn, '-', sn, '-', localcellid)) as TotalCells,
                            COUNT(DISTINCT CASE WHEN run_off = 1 OR run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecuted,
                            COUNT(DISTINCT CASE WHEN (run_off IS NULL OR run_off = 0) AND (run_on IS NULL OR run_on = 0) THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsNotExecuted,
                            COUNT(DISTINCT CASE WHEN run_off = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOff,
                            COUNT(DISTINCT CASE WHEN run_on = 1 THEN CONCAT(srn, '-', cn, '-', sn, '-', localcellid) END) as CellsExecutedOn,
                            COALESCE(SUM(EXTRACT(EPOCH FROM (time_run_on - time_run_off)) / 3600), 0) as TotalExecutionHours
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                            AND time_run_off IS NOT NULL 
                            AND time_run_on IS NOT NULL
                        GROUP BY DATE(time)
                        ORDER BY DATE(time)";
                    
                    var dailyStats = await connection.QueryAsync<R008_DailyStatistics>(
                        dailySql,
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    result = result ?? new R008DashboardResponse();
                    result.DailyStatistics = dailyStats.ToList();
                    
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDashboardByMonthAsync: {ex.Message}");
                throw;
            }
        }
        
        // Get paginated scheduler records
        public async Task<R008PagedResponse<R008_RunScheduler>> GetSchedulerRecordsPagedAsync(
            DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var countSql = @"
                        SELECT COUNT(*) 
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate";
                    
                    var totalCount = await connection.ExecuteScalarAsync<int>(
                        countSql, 
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    var dataSql = @"
                        SELECT *
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                        ORDER BY time DESC
                        LIMIT @PageSize OFFSET @Offset";
                    
                    var offset = (page - 1) * pageSize;
                    var data = await connection.QueryAsync<R008_RunScheduler>(
                        dataSql,
                        new { StartDate = startDate, EndDate = endDate, PageSize = pageSize, Offset = offset }
                    );
                    
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    
                    return new R008PagedResponse<R008_RunScheduler>
                    {
                        Data = data.ToList(),
                        TotalCount = totalCount,
                        TotalPages = totalPages,
                        CurrentPage = page,
                        PageSize = pageSize
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSchedulerRecordsPagedAsync: {ex.Message}");
                throw;
            }
        }
        
        // Get statistics by specific date
        public async Task<R008DashboardResponse> GetStatisticsByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = date.Date.AddDays(1).AddSeconds(-1);
            return await GetDashboardByDayAsync(startDate, endDate);
        }
        
        // Get records by cell name
        public async Task<List<R008_RunScheduler>> GetRecordsByCellNameAsync(
            string cellName, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var sql = @"
                        SELECT *
                        FROM r008_run_scheduler
                        WHERE cell_name = @CellName 
                            AND time >= @StartDate 
                            AND time <= @EndDate
                        ORDER BY time DESC";
                    
                    var data = await connection.QueryAsync<R008_RunScheduler>(
                        sql,
                        new { CellName = cellName, StartDate = startDate, EndDate = endDate }
                    );
                    
                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRecordsByCellNameAsync: {ex.Message}");
                throw;
            }
        }
        
        // Export to CSV
        public async Task<string> ExportSchedulerRecordsToCsvAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var sql = @"
                        SELECT 
                            id as Id,
                            time as Time,
                            cell_name as CellName,
                            enodeb_name as EnodebName,
                            srn as Srn,
                            cn as Cn,
                            sn as Sn,
                            localcellid as LocalCellId,
                            run_off as RunOff,
                            run_on as RunOn,
                            time_run_off as TimeRunOff,
                            time_run_on as TimeRunOn
                        FROM r008_run_scheduler
                        WHERE time >= @StartDate AND time <= @EndDate
                        ORDER BY time DESC, srn, cn, sn, localcellid";
                    
                    var data = await connection.QueryAsync<R008_RunScheduler>(
                        sql,
                        new { StartDate = startDate, EndDate = endDate }
                    );
                    
                    var csv = new StringBuilder();
                    // UTF-8 BOM for Excel compatibility
                    csv.Append('\uFEFF');
                    csv.AppendLine("Id,Time,CellName,EnodebName,Srn,Cn,Sn,LocalCellId,RunOff,RunOn,TimeRunOff,TimeRunOn,DurationHours");
                    
                    foreach (var record in data)
                    {
                        var timeStr = record.Time.HasValue ? record.Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                        var timeRunOffStr = record.TimeRunOff.HasValue ? record.TimeRunOff.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                        var timeRunOnStr = record.TimeRunOn.HasValue ? record.TimeRunOn.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                        var durationStr = record.DurationHours.HasValue ? record.DurationHours.Value.ToString("F2") : "";
                        
                        csv.AppendLine($"{record.Id}," +
                            $"{timeStr}," +
                            $"\"{record.CellName ?? ""}\"," +
                            $"\"{record.EnodebName ?? ""}\"," +
                            $"{record.Srn ?? 0}," +
                            $"{record.Cn ?? 0}," +
                            $"{record.Sn ?? 0}," +
                            $"\"{record.LocalCellId ?? ""}\"," +
                            $"{record.RunOff ?? 0}," +
                            $"{record.RunOn ?? 0}," +
                            $"{timeRunOffStr}," +
                            $"{timeRunOnStr}," +
                            $"{durationStr}");
                    }
                    
                    return csv.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ExportSchedulerRecordsToCsvAsync: {ex.Message}");
                throw;
            }
        }
    }
}
