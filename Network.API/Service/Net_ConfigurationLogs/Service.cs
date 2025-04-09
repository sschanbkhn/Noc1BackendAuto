using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.API.Service;
using Network.API.ViewModel.Net_ConfigurationLogs;
using Network.API.ViewModel.Net_Devices;

namespace Network.API.Service.Net_ConfigurationLogs
{
    public class Service : RepositoryBase<Model.Net_ConfigurationLogs>, Net_ConfigurationLogs.IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService) : base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }

        public async Task<List<ConfigurationLogsList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_ConfigurationLogs
                        join y in _dbContext.Net_Devices on x.DeviceId equals y.Id
                        into sub_Net_Devices
                        from yy in sub_Net_Devices.DefaultIfEmpty()
                        select new ConfigurationLogsList
                        {
                            Id = x.Id,
                            Device = yy != null ? yy.Name : null,
                            BeforeConfig = x.BeforeConfig,
                            AfterConfig = x.AfterConfig,
                            Time = x.Time != null ? x.Time.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                        };
            return await query.ToListAsync();
        }
    }
}
