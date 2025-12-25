using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.API.ViewModel.Net_DevicePorts;

namespace Network.API.Service.Net_DevicePorts
{
    public class Service : RepositoryBase<Model.Net_DevicePorts>, Net_DevicePorts.IService
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

        public async Task<List<Model.Net_DevicePorts>> GetListByDeviceIdAsync(Guid DeviceId)
        {
            var items = await _dbContext.Net_DevicePorts.Where(o => o.DeviceId == DeviceId).Select(o => new Model.Net_DevicePorts() { Id = o.Id, Name = o.Name}).ToListAsync();

            return items;
        }

        public async Task<List<DevicePortsList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_DevicePorts
                        join y in _dbContext.Net_Devices on x.DeviceId equals y.Id
                        into sub_Net_Devices
                        from yy in sub_Net_Devices.DefaultIfEmpty()
                        select new DevicePortsList
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Device = yy != null ? yy.Name : null,
                            SerialPort = x.SerialPort,
                            PortFormat = x.PortFormat,
                            Type = x.Type,
                            Status = x.Status
                        };
            return await query.ToListAsync();
        }
    }
}
