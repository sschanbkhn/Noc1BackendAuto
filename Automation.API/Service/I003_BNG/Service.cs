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

namespace Network.API.Service.I003_BNG
{
    public class Service : RepositoryBase<Model.I003_BNG>, IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Service> _logger;
        private readonly string _inocConnectionString;
        // Mới — đọc từ config
        private readonly IConfiguration _configuration;

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
             _inocConnectionString = _configuration.GetConnectionString("I003_InocConnectionString");
        }
        
        public async Task<List<Model.I003_BNG>> GetBNGDataAsync()
        {
            var results = new List<Model.I003_BNG>();
            
            try
            {
                var connectionString = _configuration.GetConnectionString("I003_InocConnectionString");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration["I003_InocConnectionString"];
                }
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("I003_InocConnectionString not found in configuration");
                }
                
                _logger.LogInformation($"Using connection string: {connectionString}");
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT 
                        a.""location"",
                        a.province_name, 
                        a.bng_name,
                        a.bng_ip::text as bng_ip, 
                        a.bng_over_session, 
                        a.bng_cleared_session, 
                        a.bng_clear_frequency 
                    FROM inoc1_db_pppoe.bng a
                    ORDER BY a.province_name, a.bng_name";
                
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var item = new Model.I003_BNG
                    {
                        location = reader.IsDBNull("location") ? null : reader.GetString("location"),
                        province_name = reader.IsDBNull("province_name") ? null : reader.GetString("province_name"),
                        bng_name = reader.IsDBNull("bng_name") ? null : reader.GetString("bng_name"),
                        bng_ip = reader.IsDBNull("bng_ip") ? null : reader.GetString("bng_ip"),
                        bng_over_session = reader.IsDBNull("bng_over_session") ? null : reader.GetInt32("bng_over_session"),
                        bng_cleared_session = reader.IsDBNull("bng_cleared_session") ? null : reader.GetInt32("bng_cleared_session"),
                        bng_clear_frequency = reader.IsDBNull("bng_clear_frequency") ? null : reader.GetInt32("bng_clear_frequency")
                    };
                    
                    results.Add(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting BNG data: {ex.Message}", ex);
                throw new Exception($"Error getting BNG data: {ex.Message}", ex);
            }
            
            return results;
        }

        public async Task<dynamic> ClearOverLimitSessionAsync(string ip)
        {
            try
            {
                // If no HttpClientFactory, create a basic HttpClient
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
                    // Set headers
                    httpClient.DefaultRequestHeaders.Add("X-API-KEY", "my-super-secret-key-BNG@-2025");
                    
                    // Create request payload
                    var payload = new { ip = ip };
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    
                    // Make API call
                    // var response = await httpClient.PostAsync("http://10.155.43.203:8000/api/clear_over_limit_one_bng/", content);
                    // ServiceApiUrlInoc_ConnectionString

                    // Dùng trong method
                    var ServiceApiUrlInoc_Connection_String = _configuration["ExternalServices:ServiceApiUrlInoc_Connection_String"];
                    var response = await httpClient.PostAsync($"{ServiceApiUrlInoc_ConnectionString}/api/clear_over_limit_one_bng/", content);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject(responseContent);
                        
                        _logger.LogInformation($"Successfully called clear API for IP: {ip}. Response: {responseContent}");
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to call clear API for IP: {ip}. Status: {response.StatusCode}, Error: {errorContent}");
                        throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling clear API for IP {ip}: {ex.Message}", ex);
                throw new Exception($"Error calling clear API for IP {ip}: {ex.Message}", ex);
            }
        }
        
        public async Task<dynamic> CheckOneUserAsync(string username, string ip)
        {
            try
            {
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
                    // Set headers
                    httpClient.DefaultRequestHeaders.Add("X-API-KEY", "my-super-secret-key-BNG@-2025");
                    
                    // Create request payload
                    var payload = new { username = username, ip = ip };
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Make API call
                    // var response = await httpClient.PostAsync("http://10.155.43.203:8000/api/check_one_user/", content);
                    // Dùng trong method
                    var ServiceApiUrlInoc_Connection_String = _configuration["ExternalServices:ServiceApiUrlInoc_Connection_String"];
                    var response = await httpClient.PostAsync($"{ServiceApiUrlInoc_ConnectionString}/api/check_one_user/", content);




                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject(responseContent);
                        
                        _logger.LogInformation($"Successfully called check user API for Username: {username}, IP: {ip}. Response: {responseContent}");
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to call check user API for Username: {username}, IP: {ip}. Status: {response.StatusCode}, Error: {errorContent}");
                        throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling check user API for Username {username}, IP {ip}: {ex.Message}", ex);
                throw new Exception($"Error calling check user API for Username {username}, IP {ip}: {ex.Message}", ex);
            }
        }
        
        public async Task<dynamic> ClearOverLimitOneUserAsync(string username, string ip)
        {
            try
            {
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
                    // Set headers
                    httpClient.DefaultRequestHeaders.Add("X-API-KEY", "my-super-secret-key-BNG@-2025");
                    
                    // Create request payload
                    var payload = new { username = username, ip = ip };
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Make API call
                    // var response = await httpClient.PostAsync("http://10.155.43.203:8000/api/clear_over_limit_one_user/", content);
                    // Dùng trong method
                    var ServiceApiUrlInoc_Connection_String = _configuration["ExternalServices:ServiceApiUrlInoc_Connection_String"];
                    var response = await httpClient.PostAsync($"{ServiceApiUrlInoc_ConnectionString}/api/clear_over_limit_one_user/", content);




                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject(responseContent);
                        
                        _logger.LogInformation($"Successfully called clear over limit user API for Username: {username}, IP: {ip}. Response: {responseContent}");
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to call clear over limit user API for Username: {username}, IP: {ip}. Status: {response.StatusCode}, Error: {errorContent}");
                        throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling clear over limit user API for Username {username}, IP {ip}: {ex.Message}", ex);
                throw new Exception($"Error calling clear over limit user API for Username {username}, IP {ip}: {ex.Message}", ex);
            }
        }
        
        public async Task<dynamic> ClearAllOneUserAsync(string username, string ip)
        {
            try
            {
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
                    // Set headers
                    httpClient.DefaultRequestHeaders.Add("X-API-KEY", "my-super-secret-key-BNG@-2025");
                    
                    // Create request payload
                    var payload = new { username = username, ip = ip };
                    var jsonContent = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    
                    // Make API call
                    // var response = await httpClient.PostAsync("http://10.155.43.203:8000/api/clear_all_one_user/", content);

                    // Dùng trong method
                    var ServiceApiUrlInoc_Connection_String = _configuration["ExternalServices:ServiceApiUrlInoc_Connection_String"];
                    var response = await httpClient.PostAsync($"{ServiceApiUrlInoc_ConnectionString}/api/clear_all_one_user/", content);




                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject(responseContent);
                        
                        _logger.LogInformation($"Successfully called clear all user API for Username: {username}, IP: {ip}. Response: {responseContent}");
                        return result;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Failed to call clear all user API for Username: {username}, IP: {ip}. Status: {response.StatusCode}, Error: {errorContent}");
                        throw new Exception($"API call failed with status {response.StatusCode}: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling clear all user API for Username {username}, IP {ip}: {ex.Message}", ex);
                throw new Exception($"Error calling clear all user API for Username {username}, IP {ip}: {ex.Message}", ex);
            }
        }
        public async Task<dynamic> GetDashboardDataAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("I003_InocConnectionString");

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = _configuration["I003_InocConnectionString"];
                }

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("I003_InocConnectionString not found in configuration");
                }

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Lấy ngày, bỏ phần giờ, phút, giây
                var from = fromDate?.Date ?? DateTime.Today.Date;
                var to = toDate?.Date ?? DateTime.Today.Date;

                // --- 1. Summary Data ---
                // Tính tổng trong khoảng thời gian từ ngày - đến ngày
                var daysDiff = Math.Max(1, (to - from).Days + 1);
                var summaryQuery = @"
            SELECT
              COUNT(*) * 1000 * @daysDiff AS TongSoPhienBaoXacThuc,
              COALESCE(SUM(bng_over_session), 0) * @daysDiff AS VuotPhien,
              COALESCE(SUM(bng_cleared_session), 0) * @daysDiff AS DaXoa,
              (COALESCE(SUM(bng_over_session), 0) - COALESCE(SUM(bng_cleared_session), 0)) * @daysDiff AS ConLai
            FROM inoc1_db_pppoe.bng";

                using var summaryCmd = new NpgsqlCommand(summaryQuery, connection);
                summaryCmd.Parameters.AddWithValue("daysDiff", daysDiff);

                var summary = new
                {
                    TongSoPhienBaoXacThuc = 0L,
                    VuotPhien = 0L,
                    DaXoa = 0L,
                    ConLai = 0L
                };

                using (var reader = await summaryCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        summary = new
                        {
                            TongSoPhienBaoXacThuc = reader.IsDBNull(reader.GetOrdinal("TongSoPhienBaoXacThuc")) ? 0L : reader.GetInt64(reader.GetOrdinal("TongSoPhienBaoXacThuc")),
                            VuotPhien = reader.IsDBNull(reader.GetOrdinal("VuotPhien")) ? 0L : reader.GetInt64(reader.GetOrdinal("VuotPhien")),
                            DaXoa = reader.IsDBNull(reader.GetOrdinal("DaXoa")) ? 0L : reader.GetInt64(reader.GetOrdinal("DaXoa")),
                            ConLai = reader.IsDBNull(reader.GetOrdinal("ConLai")) ? 0L : reader.GetInt64(reader.GetOrdinal("ConLai"))
                        };
                    }
                }

                // --- 2. Top Provinces ---
                // Lấy top tỉnh theo khoảng thời gian từ ngày - đến ngày
                var topProvincesQuery = @"
            WITH province_data AS (
                SELECT
                  province_name AS ProvinceName,
                  COALESCE(SUM(bng_over_session), 0) AS VuotPhien,
                  COALESCE(SUM(bng_cleared_session), 0) AS DaXoa,
                  COUNT(bng_ip) AS BngCount,
                  -- Tính tổng vượt phiên trong khoảng thời gian (giả lập theo số ngày)
                  COALESCE(SUM(bng_over_session), 0) * @daysDiff AS TotalVuotPhienInPeriod,
                  COALESCE(SUM(bng_cleared_session), 0) * @daysDiff AS TotalDaXoaInPeriod
                FROM inoc1_db_pppoe.bng
                WHERE province_name IS NOT NULL AND province_name != ''
                GROUP BY province_name
            )
            SELECT
              ProvinceName,
              TotalVuotPhienInPeriod AS VuotPhien,
              TotalDaXoaInPeriod AS DaXoa,
              BngCount
            FROM province_data
            ORDER BY VuotPhien DESC
            LIMIT 5";

                using var provincesCmd = new NpgsqlCommand(topProvincesQuery, connection);
                provincesCmd.Parameters.AddWithValue("daysDiff", Math.Max(1, (to - from).Days + 1));

                var topProvinces = new List<object>();
                using (var reader = await provincesCmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        topProvinces.Add(new
                        {
                            ProvinceName = reader.IsDBNull(reader.GetOrdinal("ProvinceName")) ? "N/A" : reader.GetString(reader.GetOrdinal("ProvinceName")),
                            VuotPhien = reader.IsDBNull(reader.GetOrdinal("VuotPhien")) ? 0L : reader.GetInt64(reader.GetOrdinal("VuotPhien")),
                            DaXoa = reader.IsDBNull(reader.GetOrdinal("DaXoa")) ? 0L : reader.GetInt64(reader.GetOrdinal("DaXoa")),
                            BngCount = reader.IsDBNull(reader.GetOrdinal("BngCount")) ? 0L : reader.GetInt64(reader.GetOrdinal("BngCount"))
                        });
                    }
                }

                // --- 3. Chart Data ---
                // Tạo dữ liệu mẫu cho chart khi không có dữ liệu thực từ bng_dashboard_daily
                var chartData = new List<object>();
                
                // Tạo dữ liệu cho khoảng ngày được chọn
                var currentDate = from;
                var random = new Random();
                
                while (currentDate <= to)
                {
                    var baseValue = 50000 + random.Next(0, 100000);
                    var vuotPhien = 5000 + random.Next(0, 15000);
                    var daXoa = (int)(vuotPhien * 0.3) + random.Next(0, 2000);
                    
                    chartData.Add(new
                    {
                        Date = currentDate.ToString("dd/MM"),
                        VuotPhien = vuotPhien,
                        DaXoa = daXoa,
                        XacThuc = baseValue - vuotPhien
                    });
                    
                    currentDate = currentDate.AddDays(1);
                }

                // --- Trả về kết quả cuối cùng ---
                var result = new
                {
                    Summary = summary,
                    TopProvinces = topProvinces,
                    ChartData = chartData,
                    FromDate = from,
                    ToDate = to
                };

                _logger.LogInformation($"Successfully retrieved dashboard data for period {from} to {to}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting dashboard data: {ex.Message}", ex);
                throw new Exception($"Error getting dashboard data: {ex.Message}", ex);
            }
        }
        
        public async Task<List<dynamic>> GetLocationListAsync()
        {
            var results = new List<dynamic>();
            
            try
            {
                using var connection = new NpgsqlConnection(_inocConnectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT DISTINCT 
                        a.""location"",
                        a.province_name as bng_name
                    FROM inoc1_db_pppoe.bng a  
                    WHERE a.""location"" IS NOT NULL AND a.""location"" != ''
                    ORDER BY a.""location""";
                
                using var command = new NpgsqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    results.Add(new
                    {
                        location = reader.IsDBNull("location") ? null : reader.GetString("location"),
                        bng_name = reader.IsDBNull("bng_name") ? null : reader.GetString("bng_name")
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting location list: {ex.Message}", ex);
                throw new Exception($"Error getting location list: {ex.Message}", ex);
            }
            
            return results;
        }
        
        public async Task<List<dynamic>> GetBNGDataByLocationAsync(string location, DateTime? reportDate = null)
        {
            var results = new List<dynamic>();
            
            try
            {
                using var connection = new NpgsqlConnection(_inocConnectionString);
                await connection.OpenAsync();
                
                var date = reportDate?.Date ?? DateTime.Today;
                
                // Sử dụng câu lệnh SQL với bng_name như bạn yêu cầu
                var query = @"
                    SELECT b.""location"",b.province_name,b.bng_name, a.total_sessions as XacThuc, 
                           a.over_sessions as VuotPhien,a.cleared_sessions as DaXoa
                    FROM inoc1_db_pppoe.bng_dashboard_daily a 
                    INNER JOIN inoc1_db_pppoe.bng b on a.bng_ip = b.bng_ip
                    WHERE b.""location"" = @location and a.report_date = @reportDate";
                
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("reportDate", date);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    results.Add(new
                    {
                        location = reader.IsDBNull("location") ? null : reader.GetString("location"),
                        province_name = reader.IsDBNull("province_name") ? null : reader.GetString("province_name"),
                        bng_name = reader.IsDBNull("bng_name") ? null : reader.GetString("bng_name"),
                        XacThuc = reader.IsDBNull("XacThuc") ? 0 : reader.GetInt32("XacThuc"),
                        VuotPhien = reader.IsDBNull("VuotPhien") ? 0 : reader.GetInt32("VuotPhien"),
                        DaXoa = reader.IsDBNull("DaXoa") ? 0 : reader.GetInt32("DaXoa")
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting BNG data by location {location}: {ex.Message}", ex);
                throw new Exception($"Error getting BNG data by location {location}: {ex.Message}", ex);
            }
            
            return results;
        }
        
        public async Task<dynamic> GetSessionUserDashboardDataAsync(DateTime? reportDate = null, string location = null)
        {
            try
            {
                using var connection = new NpgsqlConnection(_inocConnectionString);
                await connection.OpenAsync();

                var date = reportDate?.Date ?? DateTime.Today.Date;

                // Nếu location là null hoặc "all", lấy tổng của tất cả tỉnh
                // Nếu location cụ thể, lấy dữ liệu chi tiết của tỉnh đó
                
                string query;
                NpgsqlCommand cmd;
                var data = new List<dynamic>();
                
                if (string.IsNullOrEmpty(location) || location.ToLower() == "all")
                {
                    // Lấy tổng toàn bộ tất cả tỉnh
                    query = @"
                        SELECT
                          'TOTAL' AS location,
                          COALESCE(SUM(a.total_users), 0) AS total_users,
                          COALESCE(SUM(a.total_sessions), 0) AS total_sessions,
                          COALESCE(SUM(a.limit_sessions), 0) AS limit_sessions,
                          COALESCE(SUM(a.cleared_sessions), 0) AS cleared_sessions,
                          COALESCE(SUM(a.cleared_users), 0) AS cleared_users,
                          COALESCE(SUM(a.over_sessions), 0) AS over_sessions,
                          COALESCE(SUM(a.over_users), 0) AS over_users,
                          a.report_date
                        FROM inoc1_db_pppoe.bng_dashboard_daily a
                        WHERE a.report_date = @reportDate
                        GROUP BY a.report_date";
                    
                    cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("reportDate", date);
                }
                else
                {
                    // Lấy dữ liệu chi tiết của tỉnh cụ thể
                    query = @"
                        SELECT
                          b.""location"",
                          a.id,
                          a.bng_ip,
                          a.total_users,
                          a.total_sessions,
                          a.limit_sessions,
                          a.cleared_sessions,
                          a.cleared_users,
                          a.over_sessions,
                          a.over_users,
                          a.report_date,
                          a.created_at
                        FROM inoc1_db_pppoe.bng_dashboard_daily a
                        INNER JOIN inoc1_db_pppoe.bng b ON a.bng_ip = b.bng_ip
                        WHERE a.report_date = @reportDate AND b.""location"" = @location
                        ORDER BY a.bng_ip";
                    
                    cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("reportDate", date);
                    cmd.Parameters.AddWithValue("location", location);
                }
                
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (string.IsNullOrEmpty(location) || location.ToLower() == "all")
                        {
                            data.Add(new
                            {
                                location = reader.IsDBNull("location") ? null : reader.GetString("location"),
                                total_users = reader.IsDBNull("total_users") ? 0L : reader.GetInt64("total_users"),
                                total_sessions = reader.IsDBNull("total_sessions") ? 0L : reader.GetInt64("total_sessions"),
                                limit_sessions = reader.IsDBNull("limit_sessions") ? 0L : reader.GetInt64("limit_sessions"),
                                cleared_sessions = reader.IsDBNull("cleared_sessions") ? 0L : reader.GetInt64("cleared_sessions"),
                                cleared_users = reader.IsDBNull("cleared_users") ? 0L : reader.GetInt64("cleared_users"),
                                over_sessions = reader.IsDBNull("over_sessions") ? 0L : reader.GetInt64("over_sessions"),
                                over_users = reader.IsDBNull("over_users") ? 0L : reader.GetInt64("over_users"),
                                report_date = reader.IsDBNull("report_date") ? (DateTime?)null : reader.GetDateTime("report_date")
                            });
                        }
                        else
                        {
                            data.Add(new
                            {
                                id = reader.IsDBNull("id") ? 0 : reader.GetInt32("id"),
                                location = reader.IsDBNull("location") ? null : reader.GetString("location"),
                                bng_ip = reader.IsDBNull("bng_ip") ? null : reader.GetString("bng_ip"),
                                total_users = reader.IsDBNull("total_users") ? 0 : reader.GetInt32("total_users"),
                                total_sessions = reader.IsDBNull("total_sessions") ? 0 : reader.GetInt32("total_sessions"),
                                limit_sessions = reader.IsDBNull("limit_sessions") ? 0 : reader.GetInt32("limit_sessions"),
                                cleared_sessions = reader.IsDBNull("cleared_sessions") ? 0 : reader.GetInt32("cleared_sessions"),
                                cleared_users = reader.IsDBNull("cleared_users") ? 0 : reader.GetInt32("cleared_users"),
                                over_sessions = reader.IsDBNull("over_sessions") ? 0 : reader.GetInt32("over_sessions"),
                                over_users = reader.IsDBNull("over_users") ? 0 : reader.GetInt32("over_users"),
                                report_date = reader.IsDBNull("report_date") ? (DateTime?)null : reader.GetDateTime("report_date"),
                                created_at = reader.IsDBNull("created_at") ? (DateTime?)null : reader.GetDateTime("created_at")
                            });
                        }
                    }
                }

                var result = new
                {
                    data = data,
                    reportDate = date,
                    location = string.IsNullOrEmpty(location) ? "ALL" : location
                };

                _logger.LogInformation($"Successfully retrieved session user dashboard data for date {date}, location: {location}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting session user dashboard data: {ex.Message}", ex);
                throw new Exception($"Error getting session user dashboard data: {ex.Message}", ex);
            }
        }
    }
}
public record SummaryData
{
    public long TongSoPhienBaoXacThuc { get; init; }
    public long VuotPhien { get; init; }
    public long DaXoa { get; init; }
    public long ConLai { get; init; }
}