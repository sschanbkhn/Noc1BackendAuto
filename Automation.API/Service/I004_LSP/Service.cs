using Network.API.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service
{
    public partial class I004_LSP
    {
        public class Service : IService
        {
            private readonly ILogger<I004_LSP.Service> _logger;
            private readonly IConfiguration _configuration;
            private readonly string _inocConnectionString;

            public Service(ILogger<I004_LSP.Service> logger, IConfiguration configuration)
            {
                // Tạo console logger nếu logger là null
                if (logger == null)
                {
                    var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
                        builder.AddConsole().SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information));
                    _logger = loggerFactory.CreateLogger<I004_LSP.Service>();
                }
                else
                {
                    _logger = logger;
                }

                _configuration = configuration;
                _inocConnectionString = _configuration.GetConnectionString("InocConnectionString");

                _logger.LogInformation($"🔥 I004_LSP Service constructed. Connection string length: {_inocConnectionString?.Length ?? 0}");
                _logger.LogInformation($"🔥 Connection string exists: {!string.IsNullOrEmpty(_inocConnectionString)}");
            }

            public async Task<List<LSPInternationalDataDto>> GetLSPInternationalDataAsync(DateTime fromDate, DateTime toDate)
            {
                try
                {
                    var sql = @"
                        SELECT a.name,
                               a.from_address::text as from_address,
                               b.host_name as host_name_from,
                               a.to_address::text as to_address,
                               c.host_name as host_name_to,
                               a.""action"",
                               a.operational_status,
                               round(byte_to_gb(a.bandwidth),2) as bandwidth,
                               a.path_lsp,
                               a.last_update
                        FROM lsps a 
                        JOIN routernode b on b.id_node = a.from_address
                        JOIN routernode c on c.id_node = a.to_address
                        WHERE a.last_update >= @fromDate AND a.last_update <= @toDate
                        ORDER BY a.last_update desc";

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();
                        using (var command = new Npgsql.NpgsqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                            command.Parameters.AddWithValue("@toDate", toDate);

                            var result = new List<LSPInternationalDataDto>();
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    result.Add(new LSPInternationalDataDto
                                    {
                                        Name = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                        FromAddress = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                        HostNameFrom = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                        ToAddress = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                        HostNameTo = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                        Action = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                        OperationalStatus = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                        Bandwidth = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                                        PathLsp = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                        LastUpdate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9)
                                    });
                                }
                            }
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetLSPInternationalDataAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<List<RouterNodeDto>> GetPDataListAsync()
            {
                try
                {
                    // Trả về TẤT CẢ router nodes để user có thể chọn bất kỳ router nào
                    var sql = @"SELECT a.host_name::text, a.id_node::text 
                               FROM routernode a 
                               ORDER BY a.host_name";

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();
                        using (var command = new Npgsql.NpgsqlCommand(sql, connection))
                        {
                            var result = new List<RouterNodeDto>();
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var hostName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                    var idNode = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                    if (!string.IsNullOrEmpty(hostName) && !string.IsNullOrEmpty(idNode))
                                    {
                                        result.Add(new RouterNodeDto
                                        {
                                            HostName = hostName,
                                            IdNode = idNode
                                        });
                                    }
                                }
                            }
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetPDataListAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<List<RouterNodeDto>> GetPOPDataListAsync()
            {
                try
                {
                    // Trả về tất cả router có pcep_address (không cần filter theo dữ liệu LSP)
                    // Để user có thể chọn bất kỳ router PCEP nào
                    //var sql = @"SELECT a.host_name::text, a.id_node::text 
                    //           FROM routernode a 
                    //           WHERE a.pcep_address IS NOT NULL
                    //           ORDER BY a.host_name";
                    var sql = @"SELECT a.host_name::text, a.id_node::text 
                               FROM routernode a 
                              
                               ORDER BY a.host_name";

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();
                        using (var command = new Npgsql.NpgsqlCommand(sql, connection))
                        {
                            var result = new List<RouterNodeDto>();
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var hostName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                    var idNode = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                    if (!string.IsNullOrEmpty(hostName) && !string.IsNullOrEmpty(idNode))
                                    {
                                        result.Add(new RouterNodeDto
                                        {
                                            HostName = hostName,
                                            IdNode = idNode
                                        });
                                    }
                                }
                            }
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetPOPDataListAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<RoutePCEPStatusDto> GetRoutePCEPStatusAsync()
            {
                try
                {
                    var result = new RoutePCEPStatusDto();

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();

                        // Get UP count
                        var upSql = "SELECT count(*) FROM routernode a WHERE a.pcep_operational_status = 'Up'";
                        using (var command = new Npgsql.NpgsqlCommand(upSql, connection))
                        {
                            result.UpCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // Get DOWN count
                        var downSql = "SELECT count(*) FROM routernode a WHERE a.pcep_operational_status = 'Down'";
                        using (var command = new Npgsql.NpgsqlCommand(downSql, connection))
                        {
                            result.DownCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetRoutePCEPStatusAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<LSPDelegatedStatusDto> GetLSPDelegatedStatusAsync()
            {
                try
                {
                    var result = new LSPDelegatedStatusDto();

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();

                        // Get Active count
                        var activeSql = "SELECT count(*) FROM current_lsp a WHERE a.operational_status = 'Active'";
                        using (var command = new Npgsql.NpgsqlCommand(activeSql, connection))
                        {
                            result.ActiveCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // Get Down count
                        var downSql = "SELECT count(*) FROM current_lsp a WHERE a.operational_status = 'Down'";
                        using (var command = new Npgsql.NpgsqlCommand(downSql, connection))
                        {
                            result.DownCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // Get Unknown count
                        var unknownSql = "SELECT count(*) FROM current_lsp a WHERE a.operational_status = 'Unknown'";
                        using (var command = new Npgsql.NpgsqlCommand(unknownSql, connection))
                        {
                            result.UnknownCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetLSPDelegatedStatusAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<LSPActionStatsDto> GetLSPActionStatsAsync(DateTime fromDate, DateTime toDate)
            {
                try
                {
                    var result = new LSPActionStatsDto();

                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        await connection.OpenAsync();

                        // Get Add count
                        var addSql = "SELECT count(*) FROM lsps a WHERE a.action = 'add' AND a.last_update >= @fromDate AND a.last_update <= @toDate";
                        using (var command = new Npgsql.NpgsqlCommand(addSql, connection))
                        {
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                            command.Parameters.AddWithValue("@toDate", toDate);
                            result.AddCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // Get Update count
                        var updateSql = "SELECT count(*) FROM lsps a WHERE a.action = 'update' AND a.last_update >= @fromDate AND a.last_update <= @toDate";
                        using (var command = new Npgsql.NpgsqlCommand(updateSql, connection))
                        {
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                            command.Parameters.AddWithValue("@toDate", toDate);
                            result.UpdateCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        // Get Remove count
                        var removeSql = "SELECT count(*) FROM lsps a WHERE a.action = 'remove' AND a.last_update >= @fromDate AND a.last_update <= @toDate";
                        using (var command = new Npgsql.NpgsqlCommand(removeSql, connection))
                        {
                            command.Parameters.AddWithValue("@fromDate", fromDate);
                            command.Parameters.AddWithValue("@toDate", toDate);
                            result.RemoveCount = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetLSPActionStatsAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<List<LSPBandwidthDto>> GetLSPBandwidthDataAsync(string[] fromIdNodes, string[] toIdNodes, DateTime fromDate, DateTime toDate)
            {
                _logger.LogInformation($"🔥 GetLSPBandwidthDataAsync STARTED - Entry point reached");

                try
                {
                    _logger.LogInformation($"GetLSPBandwidthDataAsync called with {fromIdNodes?.Length ?? 0} fromIdNodes and {toIdNodes?.Length ?? 0} toIdNodes");
                    _logger.LogInformation($"FromIdNodes: [{string.Join(", ", fromIdNodes ?? new string[0])}]");
                    _logger.LogInformation($"ToIdNodes: [{string.Join(", ", toIdNodes ?? new string[0])}]");
                    _logger.LogInformation($"Date range: {fromDate:yyyy-MM-dd HH:mm} to {toDate:yyyy-MM-dd HH:mm}");
                    _logger.LogInformation($"Connection string exists: {!string.IsNullOrEmpty(_inocConnectionString)}");

                    var result = new List<LSPBandwidthDto>();

                    if (fromIdNodes == null || toIdNodes == null || fromIdNodes.Length == 0 || toIdNodes.Length == 0)
                    {
                        _logger.LogWarning("fromIdNodes or toIdNodes is null or empty");
                        return result;
                    }

                    _logger.LogInformation($"About to create database connection...");
                    using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                    {
                        _logger.LogInformation($"Database connection created, opening...");
                        await connection.OpenAsync();
                        _logger.LogInformation($"Database connection opened successfully");

                        // Tạo list các cặp IP duy nhất để query
                        var ipPairs = new List<(string from, string to)>();

                        // Nếu cả 2 arrays giống nhau, chỉ query các cặp khác nhau (from != to)
                        if (fromIdNodes.SequenceEqual(toIdNodes))
                        {
                            _logger.LogInformation("FromIdNodes and ToIdNodes are identical, querying distinct pairs only");
                            for (int i = 0; i < fromIdNodes.Length; i++)
                            {
                                for (int j = 0; j < toIdNodes.Length; j++)
                                {
                                    if (i != j && !string.IsNullOrEmpty(fromIdNodes[i]) && !string.IsNullOrEmpty(toIdNodes[j]))
                                    {
                                        ipPairs.Add((fromIdNodes[i], toIdNodes[j]));
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Nếu khác nhau, query tất cả combinations
                            foreach (var fromIdNode in fromIdNodes)
                            {
                                foreach (var toIdNode in toIdNodes)
                                {
                                    if (!string.IsNullOrEmpty(fromIdNode) && !string.IsNullOrEmpty(toIdNode))
                                    {
                                        ipPairs.Add((fromIdNode, toIdNode));
                                    }
                                }
                            }
                        }

                        _logger.LogInformation($"Will query {ipPairs.Count} IP pairs");
                        _logger.LogInformation($"IP pairs to query: {string.Join(", ", ipPairs.Select(p => $"{p.from}→{p.to}"))}");

                        // ⚠️ IMPORTANT: CHECK IF WE'RE GETTING TOO MANY PAIRS
                        if (ipPairs.Count > 10)
                        {
                            _logger.LogWarning($"🚨 WARNING: Too many IP pairs ({ipPairs.Count})! This might return too much data.");
                            _logger.LogWarning($"🚨 Consider limiting the combinations or using different logic.");
                        }

                        // Debug: Kiểm tra các bảng có thể chứa dữ liệu bandwidth
                        _logger.LogInformation("🔍 Checking available tables and data...");
                        
                        // Check bảng lsp_bandwidth_agg
                        try
                        {
                            var checkTableSql = "SELECT COUNT(*) FROM lsp_bandwidth_agg";
                            using (var checkCommand = new Npgsql.NpgsqlCommand(checkTableSql, connection))
                            {
                                var totalRecords = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                                _logger.LogInformation($"📊 lsp_bandwidth_agg table has {totalRecords} total records");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"❌ Table lsp_bandwidth_agg not found or error: {ex.Message}");
                        }

                        // Check bảng lsps (có thể có bandwidth data)
                        try
                        {
                            var checkLspsSql = "SELECT COUNT(*) FROM lsps WHERE bandwidth IS NOT NULL";
                            using (var checkCommand = new Npgsql.NpgsqlCommand(checkLspsSql, connection))
                            {
                                var lspRecords = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                                _logger.LogInformation($"📊 lsps table has {lspRecords} records with bandwidth data");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"❌ Table lsps not found or error: {ex.Message}");
                        }

                        // Check current_lsp table
                        try
                        {
                            var checkCurrentLspSql = "SELECT COUNT(*) FROM current_lsp";
                            using (var checkCommand = new Npgsql.NpgsqlCommand(checkCurrentLspSql, connection))
                            {
                                var currentLspRecords = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                                _logger.LogInformation($"📊 current_lsp table has {currentLspRecords} records");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"❌ Table current_lsp not found or error: {ex.Message}");
                        }

                        // Debug: Check what data exists in the table for specific IPs - more detailed
                        var detailedCheckSql = @"
                            SELECT 'lsp_bandwidth_agg' as table_name, 
                                   COUNT(*) as total_records,
                                   MIN(ts) as earliest_date,
                                   MAX(ts) as latest_date,
                                   COUNT(DISTINCT from_address) as unique_from_addresses,
                                   COUNT(DISTINCT to_address) as unique_to_addresses
                            FROM lsp_bandwidth_agg 
                            UNION ALL
                            SELECT 'lsps' as table_name,
                                   COUNT(*) as total_records,
                                   MIN(last_update) as earliest_date,
                                   MAX(last_update) as latest_date,
                                   COUNT(DISTINCT from_address) as unique_from_addresses,
                                   COUNT(DISTINCT to_address) as unique_to_addresses
                            FROM lsps WHERE bandwidth IS NOT NULL";
                        
                        using (var detailedCheckCommand = new Npgsql.NpgsqlCommand(detailedCheckSql, connection))
                        {
                            using (var detailedReader = await detailedCheckCommand.ExecuteReaderAsync())
                            {
                                while (await detailedReader.ReadAsync())
                                {
                                    var tableName = detailedReader.GetString(0);
                                    var totalRecords = detailedReader.GetInt32(1);
                                    var earliestDate = detailedReader.IsDBNull(2) ? (DateTime?)null : detailedReader.GetDateTime(2);
                                    var latestDate = detailedReader.IsDBNull(3) ? (DateTime?)null : detailedReader.GetDateTime(3);
                                    var uniqueFromAddresses = detailedReader.GetInt32(4);
                                    var uniqueToAddresses = detailedReader.GetInt32(5);
                                    
                                    _logger.LogInformation($"🔍 Table {tableName}: {totalRecords} records, date range: {earliestDate} to {latestDate}, unique from: {uniqueFromAddresses}, unique to: {uniqueToAddresses}");
                                }
                            }
                        }

                        // Sample specific IP addresses from database
                        var sampleIpSql = @"
                            SELECT DISTINCT from_address::text, to_address::text, ts
                            FROM lsp_bandwidth_agg 
                            WHERE from_address::text LIKE '123.29.%' 
                               OR to_address::text LIKE '123.29.%'
                            ORDER BY ts DESC
                            LIMIT 10";
                        
                        using (var sampleIpCommand = new Npgsql.NpgsqlCommand(sampleIpSql, connection))
                        {
                            using (var sampleIpReader = await sampleIpCommand.ExecuteReaderAsync())
                            {
                                var sampleIps = new List<string>();
                                while (await sampleIpReader.ReadAsync())
                                {
                                    var fromAddr = sampleIpReader.GetString(0);
                                    var toAddr = sampleIpReader.GetString(1);
                                    var ts = sampleIpReader.GetDateTime(2);
                                    sampleIps.Add($"{fromAddr}→{toAddr} ({ts:yyyy-MM-dd HH:mm})");
                                }
                                _logger.LogInformation($"🔍 Recent sample IPs in 123.29.x range: {string.Join(", ", sampleIps)}");
                            }
                        }

                        // Query từng cặp IP với điều kiện thời gian - Clean IPs by removing /32 suffix
                        foreach (var (fromIdNode, toIdNode) in ipPairs)
                        {
                            // Clean IPs by removing /32 suffix if present
                            var dbFromIp = fromIdNode?.Replace("/32", "")?.Trim();
                            var dbToIp = toIdNode?.Replace("/32", "")?.Trim();
                            
                            // Skip if IPs are empty after cleaning
                            if (string.IsNullOrEmpty(dbFromIp) || string.IsNullOrEmpty(dbToIp))
                            {
                                _logger.LogWarning($"Skipping empty IP pair: '{fromIdNode}' -> '{toIdNode}'");
                                continue;
                            }
                            
                            _logger.LogInformation($"Querying bandwidth for fromAddress='{dbFromIp}', toAddress='{dbToIp}' from {fromDate:yyyy-MM-dd HH:mm} to {toDate:yyyy-MM-dd HH:mm}");

                            // Try lsp_bandwidth_agg first - Match both with and without /32 suffix
                            var bandwidthSql = @"
                    SELECT a.ts, a.from_address::text, a.to_address::text, a.path_lsp, 
                           round(a.total_bandwidth::numeric / 1073741824.0, 2) as bandwidth
                    FROM lsp_bandwidth_agg a 
                    WHERE (a.from_address::text = @fromAddress OR a.from_address::text = @fromAddressWithSuffix)
                      AND (a.to_address::text = @toAddress OR a.to_address::text = @toAddressWithSuffix)
                      AND a.ts >= @fromDate 
                      AND a.ts <= @toDate
                    ORDER BY a.ts";

                            var foundRecords = false;
                            using (var bandwidthCommand = new Npgsql.NpgsqlCommand(bandwidthSql, connection))
                            {
                                bandwidthCommand.Parameters.AddWithValue("@fromAddress", dbFromIp);
                                bandwidthCommand.Parameters.AddWithValue("@fromAddressWithSuffix", dbFromIp + "/32");
                                bandwidthCommand.Parameters.AddWithValue("@toAddress", dbToIp);
                                bandwidthCommand.Parameters.AddWithValue("@toAddressWithSuffix", dbToIp + "/32");
                                bandwidthCommand.Parameters.AddWithValue("@fromDate", fromDate);
                                bandwidthCommand.Parameters.AddWithValue("@toDate", toDate);

                                _logger.LogInformation($"🔥 Executing query: {bandwidthSql}");
                                _logger.LogInformation($"🔥 Parameters: fromAddress='{dbFromIp}', toAddress='{dbToIp}', fromDate='{fromDate}', toDate='{toDate}'");

                                using (var reader = await bandwidthCommand.ExecuteReaderAsync())
                                {
                                    var count = 0;
                                    while (await reader.ReadAsync())
                                    {
                                        result.Add(new LSPBandwidthDto
                                        {
                                            Ts = reader.GetDateTime(0),
                                            FromAddress = reader.GetString(1),
                                            ToAddress = reader.GetString(2),
                                            PathLsp = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                            Bandwidth = reader.GetDecimal(4)
                                        });
                                        count++;
                                        foundRecords = true;
                                    }
                                    _logger.LogInformation($"✅ Found {count} records in lsp_bandwidth_agg for fromAddress='{dbFromIp}', toAddress='{dbToIp}'");
                                }
                            }

                            // If no records found in lsp_bandwidth_agg, try lsps table
                            if (!foundRecords)
                            {
                                _logger.LogInformation($"No data in lsp_bandwidth_agg, trying lsps table for fromAddress='{dbFromIp}', toAddress='{dbToIp}'");
                                
                                var lspsSql = @"
                        SELECT a.last_update as ts, a.from_address::text, a.to_address::text, a.path_lsp, 
                               round(byte_to_gb(a.bandwidth),2) as bandwidth
                        FROM lsps a 
                        WHERE (a.from_address::text = @fromAddress OR a.from_address::text = @fromAddressWithSuffix)
                          AND (a.to_address::text = @toAddress OR a.to_address::text = @toAddressWithSuffix)
                          AND a.last_update >= @fromDate 
                          AND a.last_update <= @toDate
                          AND a.bandwidth IS NOT NULL
                        ORDER BY a.last_update";

                                using (var lspsCommand = new Npgsql.NpgsqlCommand(lspsSql, connection))
                                {
                                    lspsCommand.Parameters.AddWithValue("@fromAddress", dbFromIp);
                                    lspsCommand.Parameters.AddWithValue("@fromAddressWithSuffix", dbFromIp + "/32");
                                    lspsCommand.Parameters.AddWithValue("@toAddress", dbToIp);
                                    lspsCommand.Parameters.AddWithValue("@toAddressWithSuffix", dbToIp + "/32");
                                    lspsCommand.Parameters.AddWithValue("@fromDate", fromDate);
                                    lspsCommand.Parameters.AddWithValue("@toDate", toDate);

                                    _logger.LogInformation($"🔥 Fallback query: {lspsSql}");

                                    using (var lspsReader = await lspsCommand.ExecuteReaderAsync())
                                    {
                                        var count = 0;
                                        while (await lspsReader.ReadAsync())
                                        {
                                            result.Add(new LSPBandwidthDto
                                            {
                                                Ts = lspsReader.GetDateTime(0),
                                                FromAddress = lspsReader.GetString(1),
                                                ToAddress = lspsReader.GetString(2),
                                                PathLsp = lspsReader.IsDBNull(3) ? "" : lspsReader.GetString(3),
                                                Bandwidth = lspsReader.GetDecimal(4)
                                            });
                                            count++;
                                        }
                                        _logger.LogInformation($"✅ Found {count} records in lsps table for fromAddress='{dbFromIp}', toAddress='{dbToIp}'");
                                    }
                                }
                            }
                        }
                    }

                    _logger.LogInformation($"Total bandwidth records found: {result.Count}");
                    
                    // Verify that we only return data for the requested IP pairs
                    if (result.Count > 0)
                    {
                        var returnedPairs = result.Select(r => $"{r.FromAddress}→{r.ToAddress}").Distinct().ToList();
                        _logger.LogInformation($"Returned data for IP pairs: {string.Join(", ", returnedPairs)}");
                        
                        // Double-check: ensure we don't return data outside the requested scope
                        var requestedFromIPs = fromIdNodes?.ToList() ?? new List<string>();
                        var requestedToIPs = toIdNodes?.ToList() ?? new List<string>();
                        
                        foreach (var record in result)
                        {
                            var fromMatches = requestedFromIPs.Any(ip => 
                                record.FromAddress == ip || record.FromAddress == $"{ip}/32" || record.FromAddress.Replace("/32", "") == ip);
                            var toMatches = requestedToIPs.Any(ip => 
                                record.ToAddress == ip || record.ToAddress == $"{ip}/32" || record.ToAddress.Replace("/32", "") == ip);
                                
                            if (!fromMatches || !toMatches)
                            {
                                _logger.LogWarning($"⚠️ Record {record.FromAddress}→{record.ToAddress} doesn't match requested IPs");
                            }
                        }
                    }
                    
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"🔥 GetLSPBandwidthDataAsync EXCEPTION: {ex.Message}");
                    _logger.LogError($"🔥 Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
            public async Task<List<LSPBandwidthDto>> GetBandwidthByPathAsync(string[] fromData, string[] toData, string timeRange, DateTime? fromDate = null, DateTime? toDate = null)
            {
                try
                {
                    DateTime calculatedFromDate;
                    DateTime calculatedToDate = DateTime.Now;

                    // Calculate date range based on timeRange
                    switch (timeRange?.ToLower())
                    {
                        case "3h":
                            calculatedFromDate = calculatedToDate.AddHours(-3);
                            break;
                        case "12h":
                            calculatedFromDate = calculatedToDate.AddHours(-12);
                            break;
                        case "24h":
                            calculatedFromDate = calculatedToDate.AddDays(-1);
                            break;
                        case "1w":
                            calculatedFromDate = calculatedToDate.AddDays(-7);
                            break;
                        case "1m":
                            calculatedFromDate = calculatedToDate.AddDays(-30);
                            break;
                        case "manual":
                            if (!fromDate.HasValue || !toDate.HasValue)
                            {
                                throw new ArgumentException("FromDate and ToDate are required for manual range");
                            }
                            calculatedFromDate = fromDate.Value;
                            calculatedToDate = toDate.Value;
                            break;
                        default:
                            calculatedFromDate = calculatedToDate.AddDays(-1); // Default to 24h
                            break;
                    }

                    // Validate date range
                    if (calculatedFromDate >= calculatedToDate)
                    {
                        throw new ArgumentException("FromDate must be less than ToDate");
                    }

                    // Limit date range to prevent performance issues (max 90 days)
                    var maxDays = 90;
                    if ((calculatedToDate - calculatedFromDate).TotalDays > maxDays)
                    {
                        throw new ArgumentException($"Date range cannot exceed {maxDays} days");
                    }

                    _logger.LogInformation($"GetBandwidthByPathAsync: timeRange={timeRange}, calculated dates: {calculatedFromDate:yyyy-MM-dd HH:mm} to {calculatedToDate:yyyy-MM-dd HH:mm}");

                    // Gọi method GetLSPBandwidthDataAsync với thời gian đã tính toán
                    return await GetLSPBandwidthDataAsync(fromData, toData, calculatedFromDate, calculatedToDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetBandwidthByPathAsync error: {ex.Message}");
                    throw;
                }
            }

            public async Task<object> DebugDatabaseAsync()
            {
                using (var connection = new Npgsql.NpgsqlConnection(_inocConnectionString))
                {
                    await connection.OpenAsync();
                    
                    var result = new
                    {
                        DatabaseInfo = new {
                            ConnectionString = _inocConnectionString.Replace("password=", "password=***"),
                            IsConnected = connection.State == System.Data.ConnectionState.Open
                        },
                        TableStats = new List<object>()
                    };
                    
                    // Check lsp_bandwidth_agg table
                    try
                    {
                        var sql1 = "SELECT COUNT(*) as total_records, MIN(ts) as earliest_date, MAX(ts) as latest_date FROM lsp_bandwidth_agg";
                        using (var cmd = new Npgsql.NpgsqlCommand(sql1, connection))
                        {
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    var tableInfo = new {
                                        TableName = "lsp_bandwidth_agg",
                                        TotalRecords = reader.GetInt32(0),
                                        EarliestDate = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1),
                                        LatestDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2)
                                    };
                                    ((List<object>)result.TableStats).Add(tableInfo);
                                }
                            }
                        }
                        
                        // Get sample IPs from lsp_bandwidth_agg
                        var sql2 = "SELECT DISTINCT from_address::text, to_address::text FROM lsp_bandwidth_agg LIMIT 5";
                        using (var cmd2 = new Npgsql.NpgsqlCommand(sql2, connection))
                        {
                            using (var reader2 = await cmd2.ExecuteReaderAsync())
                            {
                                var sampleIPs = new List<string>();
                                while (await reader2.ReadAsync())
                                {
                                    sampleIPs.Add($"{reader2.GetString(0)} -> {reader2.GetString(1)}");
                                }
                                ((List<object>)result.TableStats).Add(new { TableName = "lsp_bandwidth_agg_samples", SampleIPs = sampleIPs });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ((List<object>)result.TableStats).Add(new { TableName = "lsp_bandwidth_agg", Error = ex.Message });
                    }
                    
                    // Check lsps table
                    try
                    {
                        var sql3 = "SELECT COUNT(*) as total_records, COUNT(*) FILTER (WHERE bandwidth IS NOT NULL) as with_bandwidth FROM lsps";
                        using (var cmd3 = new Npgsql.NpgsqlCommand(sql3, connection))
                        {
                            using (var reader3 = await cmd3.ExecuteReaderAsync())
                            {
                                if (await reader3.ReadAsync())
                                {
                                    var tableInfo = new {
                                        TableName = "lsps",
                                        TotalRecords = reader3.GetInt32(0),
                                        WithBandwidth = reader3.GetInt32(1)
                                    };
                                    ((List<object>)result.TableStats).Add(tableInfo);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ((List<object>)result.TableStats).Add(new { TableName = "lsps", Error = ex.Message });
                    }
                    
                    return result;
                }
            }
        }
    }
}
