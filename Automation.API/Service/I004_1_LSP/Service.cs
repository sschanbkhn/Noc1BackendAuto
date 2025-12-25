using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Network.API.Infrastructure;
using Network.API.Model;
using Network.Core.Interfaces;

namespace Network.API.Service.I004_1_LSP
{
    public class Service : RepositoryBase<Model.I004_1_LSP>, IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        private readonly IConfiguration _configuration;
        
        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService, IConfiguration configuration)
            : base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
            _configuration = configuration;
        }
        
        public async Task<List<Model.I004_1_LSP>> GetLSPDataAsync(DateTime fromDate, DateTime toDate)
        {
            var results = new List<Model.I004_1_LSP>();
            
            try
            {
                var connectionString = _configuration["InocConnectionString"];
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("InocConnectionString not found in configuration");
                }
                
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                
                var query = @"
                    SELECT 
                        a.name,
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
                    ORDER BY a.last_update DESC";
                
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@fromDate", fromDate);
                command.Parameters.AddWithValue("@toDate", toDate);
                
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    var item = new Model.I004_1_LSP
                    {
                        name = reader.IsDBNull("name") ? null : reader.GetString("name"),
                        from_address = reader.IsDBNull("from_address") ? null : reader.GetString("from_address"),
                        host_name_from = reader.IsDBNull("host_name_from") ? null : reader.GetString("host_name_from"),
                        to_address = reader.IsDBNull("to_address") ? null : reader.GetString("to_address"),
                        host_name_to = reader.IsDBNull("host_name_to") ? null : reader.GetString("host_name_to"),
                        action = reader.IsDBNull("action") ? null : reader.GetString("action"),
                        operational_status = reader.IsDBNull("operational_status") ? null : reader.GetString("operational_status"),
                        bandwidth = reader.IsDBNull("bandwidth") ? null : reader.GetDecimal("bandwidth"),
                        path_lsp = reader.IsDBNull("path_lsp") ? null : reader.GetString("path_lsp"),
                        last_update = reader.IsDBNull("last_update") ? null : reader.GetDateTime("last_update")
                    };
                    
                    results.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting LSP data: {ex.Message}", ex);
            }
            
            return results;
        }
    }
}
