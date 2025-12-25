using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Network.API.Infrastructure;
using Network.API.Model;
using Network.Core.Interfaces;
using Newtonsoft.Json;

namespace Network.API.Service.I002_HardwareAlarm
{
    public class Service : RepositoryBase<Model.I002_HardwareAlarm>, IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Service> _logger;
        private readonly string _inocConnectionString;

        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService, 
                      IConfiguration configuration, IHttpClientFactory httpClientFactory = null, ILogger<Service> logger = null)
            : base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<Service>.Instance;
            _inocConnectionString = _configuration.GetConnectionString("I002_InocConnectionString");
        }
        
        public async Task<List<ViewModel.I002_HardwareAlarmViewModel>> GetHardwareAlarmListAsync()
        {
            var results = new List<ViewModel.I002_HardwareAlarmViewModel>();
            
            try
            {
                var connectionString = _configuration.GetConnectionString("I002_InocConnectionString");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration["I002_InocConnectionString"];
                }
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("I002_InocConnectionString not found in configuration");
                }
                
                _logger.LogInformation($"Using connection string for I002 Hardware Alarm");
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT 
                        a.id,
                        a.device,
                        a.iploopback,
                        a.keyword,
                        a.severity,
                        a.raw_log,
                        a.fpc_slot,
                        a.fpc_sn,
                        a.fpc_pn,
                        a.fpc_desc,
                        a.fpc_ver,
                        a.fpc_model,
                        a.intf_list::text as intf_list,
                        a.can_restart,
                        a.is_active,
                        a.updated_at,
                        a.alarm_id,
                        COALESCE(r.summary_status, '') as summary_status,
                        COALESCE(r.fpc_status::text, '') as fpc_status,
                        COALESCE(r.restart_status, '') as restart_status,
                        COALESCE(r.raw_result::text, '') as raw_result
                    FROM public.hardware_alarm_detail a
                    LEFT JOIN LATERAL (
                        SELECT summary_status, fpc_status, restart_status, raw_result
                        FROM public.hardware_alarm_reset_postcheck_data
                        WHERE alarm_id = a.alarm_id
                        ORDER BY updated_at DESC
                        LIMIT 1
                    ) r ON true
                    WHERE a.is_active = 1
                    ORDER BY a.updated_at DESC";
                
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var item = new ViewModel.I002_HardwareAlarmViewModel
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id")),
                        Device = reader.IsDBNull(reader.GetOrdinal("device")) ? null : reader.GetString(reader.GetOrdinal("device")),
                        IpLoopback = reader.IsDBNull(reader.GetOrdinal("iploopback")) ? null : reader.GetString(reader.GetOrdinal("iploopback")),
                        Keyword = reader.IsDBNull(reader.GetOrdinal("keyword")) ? null : reader.GetString(reader.GetOrdinal("keyword")),
                        Severity = reader.IsDBNull(reader.GetOrdinal("severity")) ? null : reader.GetString(reader.GetOrdinal("severity")),
                        RawLog = reader.IsDBNull(reader.GetOrdinal("raw_log")) ? null : reader.GetString(reader.GetOrdinal("raw_log")),
                        FpcSlot = reader.IsDBNull(reader.GetOrdinal("fpc_slot")) ? null : reader.GetString(reader.GetOrdinal("fpc_slot")),
                        FpcSn = reader.IsDBNull(reader.GetOrdinal("fpc_sn")) ? null : reader.GetString(reader.GetOrdinal("fpc_sn")),
                        FpcPn = reader.IsDBNull(reader.GetOrdinal("fpc_pn")) ? null : reader.GetString(reader.GetOrdinal("fpc_pn")),
                        FpcDesc = reader.IsDBNull(reader.GetOrdinal("fpc_desc")) ? null : reader.GetString(reader.GetOrdinal("fpc_desc")),
                        FpcVer = reader.IsDBNull(reader.GetOrdinal("fpc_ver")) ? null : reader.GetString(reader.GetOrdinal("fpc_ver")),
                        FpcModel = reader.IsDBNull(reader.GetOrdinal("fpc_model")) ? null : reader.GetString(reader.GetOrdinal("fpc_model")),
                        IntfList = reader.IsDBNull(reader.GetOrdinal("intf_list")) ? null : reader.GetString(reader.GetOrdinal("intf_list")),
                        RawResult = reader.GetString(reader.GetOrdinal("raw_result")),
                        RestartStatus = reader.GetString(reader.GetOrdinal("restart_status")),
                        CanRestart = reader.IsDBNull(reader.GetOrdinal("can_restart")) ? null : reader.GetInt32(reader.GetOrdinal("can_restart")),
                        IsActive = reader.IsDBNull(reader.GetOrdinal("is_active")) ? null : reader.GetInt32(reader.GetOrdinal("is_active")),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? null : reader.GetDateTime(reader.GetOrdinal("updated_at")),
                        AlarmId = reader.IsDBNull(reader.GetOrdinal("alarm_id")) ? null : reader.GetInt32(reader.GetOrdinal("alarm_id")),
                        // From reset postcheck data
                        SummaryStatus = reader.GetString(reader.GetOrdinal("summary_status")),
                        FpcStatus = reader.GetString(reader.GetOrdinal("fpc_status")),
                        // Không có tracking table
                        CheckTime = null,
                        CheckUser = null,
                        StatusProcess = null
                    };
                    
                    results.Add(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting hardware alarm list: {ex.Message}", ex);
                throw new Exception($"Error getting hardware alarm list: {ex.Message}", ex);
            }
            
            return results;
        }

        public async Task<dynamic> CheckAlarmAsync(int alarmId, string username)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("I002_InocConnectionString");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration["I002_InocConnectionString"];
                }
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                
                _logger.LogInformation($"User {username} checked alarm {alarmId}");
                
                // Update time_check and user_check fields
                var updateQuery = @"
                    UPDATE public.hardware_alarm_detail 
                    SET time_check = @timeCheck, 
                        user_check = @userCheck
                    WHERE id = @alarmId";
                
                using var updateCommand = new NpgsqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("alarmId", alarmId);
                updateCommand.Parameters.AddWithValue("timeCheck", DateTime.Now);
                updateCommand.Parameters.AddWithValue("userCheck", username);
                
                await updateCommand.ExecuteNonQueryAsync();
                
                // Lấy thông tin chi tiết của alarm để trả về
                var selectQuery = @"
                    SELECT 
                        a.id,
                        a.device,
                        a.iploopback,
                        a.keyword,
                        a.severity,
                        a.fpc_slot,
                        a.fpc_desc,
                        a.updated_at,
                        a.time_check,
                        a.user_check
                    FROM public.hardware_alarm_detail a
                    WHERE a.id = @alarmDbId";
                
                using var selectCommand = new NpgsqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("alarmDbId", alarmId);
                
                using var reader = await selectCommand.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    var result = new
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Device = reader.IsDBNull(reader.GetOrdinal("device")) ? null : reader.GetString(reader.GetOrdinal("device")),
                        IpLoopback = reader.IsDBNull(reader.GetOrdinal("iploopback")) ? null : reader.GetString(reader.GetOrdinal("iploopback")),
                        Keyword = reader.IsDBNull(reader.GetOrdinal("keyword")) ? null : reader.GetString(reader.GetOrdinal("keyword")),
                        Severity = reader.IsDBNull(reader.GetOrdinal("severity")) ? null : reader.GetString(reader.GetOrdinal("severity")),
                        FpcSlot = reader.IsDBNull(reader.GetOrdinal("fpc_slot")) ? null : reader.GetString(reader.GetOrdinal("fpc_slot")),
                        FpcDesc = reader.IsDBNull(reader.GetOrdinal("fpc_desc")) ? null : reader.GetString(reader.GetOrdinal("fpc_desc")),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at")),
                        TimeCheck = reader.IsDBNull(reader.GetOrdinal("time_check")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("time_check")),
                        UserCheck = reader.IsDBNull(reader.GetOrdinal("user_check")) ? null : reader.GetString(reader.GetOrdinal("user_check"))
                    };
                        
                    _logger.LogInformation($"Successfully checked alarm {alarmId} by user {username}");
                    return result;
                }
                
                throw new Exception($"Alarm with ID {alarmId} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking alarm {alarmId}: {ex.Message}", ex);
                throw new Exception($"Error checking alarm: {ex.Message}", ex);
            }
        }

        public async Task<dynamic> AutoRebootAsync(int alarmId, string deviceName, string fpcSlot, string keyword, string username)
        {
            try
            {
                // Call webhook API để thực hiện auto reboot
                HttpClient httpClient;
                if (_httpClientFactory != null)
                {
                    httpClient = _httpClientFactory.CreateClient();
                }
                else
                {
                    httpClient = new HttpClient();
                }
                
                using (httpClient)
                {
                    var payload = new
                    {
                        //id = alarmId, // Webhook có thể expect "id" thay vì "alarm_id"
                        fpc_slot = fpcSlot?.Replace("FPC ", "")?.Trim(),
                        device_name = deviceName,
                        keyword = keyword
                    };
                    
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    
                    _logger.LogInformation($"Calling auto reboot webhook - Payload: {jsonContent}");
                    
                    // Make API call
                    var response = await httpClient.PostAsync("http://10.155.43.198:5678/webhook/f45414eb-d235-43ab-aa8d-2e7f38c94152", content);
                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Lưu kết quả vào bảng hardware_alarm_reset_postcheck_data
                    var connectionString = _configuration.GetConnectionString("I002_InocConnectionString");
                    
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        connectionString = _configuration["I002_InocConnectionString"];
                    }
                    
                    using var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();
                    
                    // Parse response để lấy data
                    string restartStatus = "Failed";
                    string summaryStatus = "Auto reboot failed";
                    string fpcStatus = null;
                    string intfStatus = null;
                    string alarms = null;
                    
                    try
                    {
                        var responseObj = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        if (responseObj?.status == "success" && responseObj?.data != null)
                        {
                            restartStatus = responseObj.data.restart_status?.ToString() ?? "Failed";
                            summaryStatus = responseObj.data.summary_status?.ToString() ?? "success";
                            
                            // Convert JSONB fields
                            if (responseObj.data.fpc_status != null)
                                fpcStatus = JsonConvert.SerializeObject(responseObj.data.fpc_status);
                            
                            if (responseObj.data.intf_status != null)
                                intfStatus = JsonConvert.SerializeObject(responseObj.data.intf_status);
                            
                            if (responseObj.data.alarms != null)
                                alarms = JsonConvert.SerializeObject(responseObj.data.alarms);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Could not parse webhook response: {ex.Message}");
                    }
                    
                    //var insertQuery = @"
                    //    INSERT INTO public.hardware_alarm_reset_postcheck_data 
                    //    (alarm_id, restart_status, fpc_status, intf_status, alarms, summary_status, updated_at)
                    //    VALUES 
                    //    (@alarmId, @restartStatus, @fpcStatus::jsonb, @intfStatus::jsonb, @alarms::jsonb, @summaryStatus, @updatedAt)";
                    
                    //using var command = new NpgsqlCommand(insertQuery, connection);
                    //command.Parameters.AddWithValue("alarmId", alarmId);
                    //command.Parameters.AddWithValue("restartStatus", restartStatus);
                    //command.Parameters.AddWithValue("fpcStatus", (object)fpcStatus ?? DBNull.Value);
                    //command.Parameters.AddWithValue("intfStatus", (object)intfStatus ?? DBNull.Value);
                    //command.Parameters.AddWithValue("alarms", (object)alarms ?? DBNull.Value);
                    //command.Parameters.AddWithValue("summaryStatus", summaryStatus);
                    //command.Parameters.AddWithValue("updatedAt", DateTime.Now);
                    
                    //await command.ExecuteNonQueryAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Successfully auto rebooted device {deviceName}. Response: {responseContent}");
                        return new
                        {
                            Success = true,
                            Message = "Auto reboot executed successfully",
                            Response = responseContent
                        };
                    }
                    else
                    {
                        _logger.LogError($"Failed to auto reboot device {deviceName}. Status: {response.StatusCode}, Error: {responseContent}");
                        return new
                        {
                            Success = false,
                            Message = $"Auto reboot failed with status {response.StatusCode}",
                            Response = responseContent
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error auto rebooting device {deviceName}: {ex.Message}", ex);
                throw new Exception($"Error auto rebooting device: {ex.Message}", ex);
            }
        }

        public async Task<dynamic> ManualHandleAsync(int alarmId, string username, string causeName = "Manual Handle")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("I002_InocConnectionString");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration["I002_InocConnectionString"];
                }
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                
                using var transaction = await connection.BeginTransactionAsync();
                
                try
                {
                    // Step 1: Get alarm detail from hardware_alarm_detail
                    var selectQuery = @"
                        SELECT 
                            device, iploopback, keyword, severity, raw_log,
                            fpc_slot, fpc_sn, fpc_pn, fpc_desc, fpc_ver, fpc_model,
                            intf_list, updated_at, alarm_id,
                            time_check, user_check
                        FROM public.hardware_alarm_detail
                        WHERE id = @alarmId";
                    
                    using var selectCommand = new NpgsqlCommand(selectQuery, connection, transaction);
                    selectCommand.Parameters.AddWithValue("alarmId", alarmId);
                    
                    using var reader = await selectCommand.ExecuteReaderAsync();
                    
                    if (!await reader.ReadAsync())
                    {
                        throw new Exception($"Alarm with ID {alarmId} not found");
                    }
                    
                    // Read all data (only fields that exist in hardware_alarm_history)
                    var device = reader.IsDBNull(reader.GetOrdinal("device")) ? null : reader.GetString(reader.GetOrdinal("device"));
                    var iploopback = reader.IsDBNull(reader.GetOrdinal("iploopback")) ? null : reader.GetString(reader.GetOrdinal("iploopback"));
                    var keyword = reader.IsDBNull(reader.GetOrdinal("keyword")) ? null : reader.GetString(reader.GetOrdinal("keyword"));
                    var severity = reader.IsDBNull(reader.GetOrdinal("severity")) ? null : reader.GetString(reader.GetOrdinal("severity"));
                    var rawLog = reader.IsDBNull(reader.GetOrdinal("raw_log")) ? null : reader.GetString(reader.GetOrdinal("raw_log"));
                    var fpcSlot = reader.IsDBNull(reader.GetOrdinal("fpc_slot")) ? null : reader.GetString(reader.GetOrdinal("fpc_slot"));
                    var fpcSn = reader.IsDBNull(reader.GetOrdinal("fpc_sn")) ? null : reader.GetString(reader.GetOrdinal("fpc_sn"));
                    var fpcPn = reader.IsDBNull(reader.GetOrdinal("fpc_pn")) ? null : reader.GetString(reader.GetOrdinal("fpc_pn"));
                    var fpcDesc = reader.IsDBNull(reader.GetOrdinal("fpc_desc")) ? null : reader.GetString(reader.GetOrdinal("fpc_desc"));
                    var fpcVer = reader.IsDBNull(reader.GetOrdinal("fpc_ver")) ? null : reader.GetString(reader.GetOrdinal("fpc_ver"));
                    var fpcModel = reader.IsDBNull(reader.GetOrdinal("fpc_model")) ? null : reader.GetString(reader.GetOrdinal("fpc_model"));
                    var intfList = reader.IsDBNull(reader.GetOrdinal("intf_list")) ? null : reader.GetString(reader.GetOrdinal("intf_list"));
                    var updatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated_at"));
                    var alarmIdValue = reader.IsDBNull(reader.GetOrdinal("alarm_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("alarm_id"));
                    var timeCheck = reader.IsDBNull(reader.GetOrdinal("time_check")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("time_check"));
                    var userCheck = reader.IsDBNull(reader.GetOrdinal("user_check")) ? null : reader.GetString(reader.GetOrdinal("user_check"));
                    
                    await reader.CloseAsync();
                    
                    // Step 2: Insert into hardware_alarm_history with cause_create
                    // Only insert fields that exist in hardware_alarm_history table
                    var insertQuery = @"
                        INSERT INTO public.hardware_alarm_history 
                        (device, iploopback, keyword, severity, raw_log, fpc_slot, fpc_sn, fpc_pn, 
                         fpc_desc, fpc_ver, fpc_model, intf_list, updated_at, 
                         alarm_id, time_check, user_check, cause_name, cause_create)
                        VALUES 
                        (@device, @iploopback, @keyword, @severity, @rawLog, @fpcSlot, @fpcSn, @fpcPn,
                         @fpcDesc, @fpcVer, @fpcModel, @intfList::jsonb, @updatedAt,
                         @alarmIdValue, @timeCheck, @userCheck, @causeName, @causeCreate)
                        RETURNING id";
                    
                    using var insertCommand = new NpgsqlCommand(insertQuery, connection, transaction);
                    insertCommand.Parameters.AddWithValue("device", (object)device ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("iploopback", (object)iploopback ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("keyword", (object)keyword ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("severity", (object)severity ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("rawLog", (object)rawLog ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcSlot", (object)fpcSlot ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcSn", (object)fpcSn ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcPn", (object)fpcPn ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcDesc", (object)fpcDesc ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcVer", (object)fpcVer ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("fpcModel", (object)fpcModel ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("intfList", (object)intfList ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("updatedAt", (object)updatedAt ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("alarmIdValue", (object)alarmIdValue ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("timeCheck", (object)timeCheck ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("userCheck", (object)userCheck ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("causeName", causeName);
                    insertCommand.Parameters.AddWithValue("causeCreate", DateTime.Now);
                    
                    var historyId = await insertCommand.ExecuteScalarAsync();
                    
                    // Step 3: Delete from hardware_alarm_detail
                    var deleteQuery = @"
                        DELETE FROM public.hardware_alarm_detail 
                        WHERE id = @alarmId";
                    
                    using var deleteCommand = new NpgsqlCommand(deleteQuery, connection, transaction);
                    deleteCommand.Parameters.AddWithValue("alarmId", alarmId);
                    
                    await deleteCommand.ExecuteNonQueryAsync();
                    
                    // Commit transaction
                    await transaction.CommitAsync();
                    
                    _logger.LogInformation($"Successfully moved alarm {alarmId} to history (ID: {historyId}) by user {username}");
                    
                    return new
                    {
                        Success = true,
                        Message = "Alarm moved to history successfully",
                        AlarmId = alarmId,
                        HistoryId = historyId,
                        Username = username,
                        CauseCreate = DateTime.Now
                    };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error moving alarm {alarmId} to history: {ex.Message}", ex);
                throw new Exception($"Error moving alarm to history: {ex.Message}", ex);
            }
        }

        public async Task<List<Model.I002_ErrorLinksStatus>> GetErrorLinksStatusAsync()
        {
            var results = new List<Model.I002_ErrorLinksStatus>();
            
            try
            {
                await using var connection = new NpgsqlConnection(_inocConnectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT 
                        id, host, interface, status, bandwidth, ae, 
                        input_rate, output_rate, created_at, ae_bandwidth, shut_link
                    FROM public.error_links_status
                    ORDER BY created_at DESC";
                
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    results.Add(new Model.I002_ErrorLinksStatus
                    {
                        Id = reader.GetInt32(0),
                        Host = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Interface = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Status = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Bandwidth = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Ae = reader.IsDBNull(5) ? null : reader.GetString(5),
                        InputRate = reader.IsDBNull(6) ? (long?)null : reader.GetInt64(6),
                        OutputRate = reader.IsDBNull(7) ? (long?)null : reader.GetInt64(7),
                        CreatedAt = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                        AeBandwidth = reader.IsDBNull(9) ? null : reader.GetString(9),
                        ShutLink = reader.IsDBNull(10) ? (bool?)null : reader.GetBoolean(10)
                    });
                }
                
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting error links status: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<Model.I002_HardwareAlarmHistory>> GetHardwareAlarmHistoryAsync()
        {
            var results = new List<Model.I002_HardwareAlarmHistory>();
            
            try
            {
                await using var connection = new NpgsqlConnection(_inocConnectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT 
                        id, device, iploopback, keyword, severity, raw_log,
                        fpc_slot, fpc_sn, fpc_pn, fpc_desc, fpc_ver, fpc_model,
                        intf_list, updated_at, alarm_id, cause_name, cause_create,
                        user_check, time_check
                    FROM public.hardware_alarm_history
                    ORDER BY cause_create DESC NULLS LAST, updated_at DESC";
                
                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    results.Add(new Model.I002_HardwareAlarmHistory
                    {
                        Id = reader.GetInt32(0),
                        Device = reader.IsDBNull(1) ? null : reader.GetString(1),
                        IpLoopback = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Keyword = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Severity = reader.IsDBNull(4) ? null : reader.GetString(4),
                        RawLog = reader.IsDBNull(5) ? null : reader.GetString(5),
                        FpcSlot = reader.IsDBNull(6) ? null : reader.GetString(6),
                        FpcSn = reader.IsDBNull(7) ? null : reader.GetString(7),
                        FpcPn = reader.IsDBNull(8) ? null : reader.GetString(8),
                        FpcDesc = reader.IsDBNull(9) ? null : reader.GetString(9),
                        FpcVer = reader.IsDBNull(10) ? null : reader.GetString(10),
                        FpcModel = reader.IsDBNull(11) ? null : reader.GetString(11),
                        IntfList = reader.IsDBNull(12) ? null : reader.GetString(12),
                        UpdatedAt = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13),
                        AlarmId = reader.IsDBNull(14) ? (int?)null : reader.GetInt32(14),
                        CauseName = reader.IsDBNull(15) ? null : reader.GetString(15),
                        CauseCreate = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16),
                        UserCheck = reader.IsDBNull(17) ? null : reader.GetString(17),
                        TimeCheck = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
                    });
                }
                
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting hardware alarm history: {ex.Message}", ex);
                throw;
            }
        }
    }
}
