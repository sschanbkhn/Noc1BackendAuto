using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Constant;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Network.API.ViewModel.Net_NetworkLinks;
using static Google.Rpc.Context.AttributeContext.Types;
using Network.API.ViewModel.Net_Devices;

namespace Network.API.Service.Net_NetworkLinks
{
    public class Service : RepositoryBase<Model.Net_NetworkLinks>, Net_NetworkLinks.IService
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

        public async Task<List<ConnectionsResult>> GetConnectionsAsync()
        {
            var items = from x in _dbContext.Net_NetworkLinks
                        join y1 in _dbContext.Net_Devices on x.HeadDeviceId equals y1.Id
                        into sub_Net_Devices1
                        from yy1 in sub_Net_Devices1.DefaultIfEmpty()
                        join y2 in _dbContext.Net_Devices on x.LastDeviceId equals y2.Id
                        into sub_Net_Devices2
                        from yy2 in sub_Net_Devices2.DefaultIfEmpty()
                        join z1 in _dbContext.Net_DevicePorts on x.HeadDevicePortId equals z1.Id
                        into sub_Net_DevicePorts1
                        from zz1 in sub_Net_DevicePorts1.DefaultIfEmpty()
                        join z2 in _dbContext.Net_DevicePorts on x.LastDevicePortId equals z2.Id
                        into sub_Net_DevicePorts2
                        from zz2 in sub_Net_DevicePorts2.DefaultIfEmpty()
                        select new ConnectionsResult
                        {
                            distance = x.Distance,
                            source = yy1 != null ? yy1.Name : "",
                            target = yy2 != null ? yy1.Name : "",
                            sourcePort = zz1 != null ? zz1.Name : "",
                            targetPort = zz2 != null ? zz2.Name : "",
                            type = x.Type == "1" ? "Cáp ngầm" : (x.Type == "2" ? "Cáp treo" : ""),
                        };
            return await items.ToListAsync();
        }

        public async Task<List<NetworkLinkList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_NetworkLinks
                        join y1 in _dbContext.Net_Devices on x.HeadDeviceId equals y1.Id
                        into sub_Net_Devices1
                        from yy1 in sub_Net_Devices1.DefaultIfEmpty()
                        join y2 in _dbContext.Net_Devices on x.LastDeviceId equals y2.Id
                        into sub_Net_Devices2
                        from yy2 in sub_Net_Devices2.DefaultIfEmpty()
                        join z1 in _dbContext.Net_DevicePorts on x.HeadDevicePortId equals z1.Id
                        into sub_Net_DevicePorts1
                        from zz1 in sub_Net_DevicePorts1.DefaultIfEmpty()
                        join z2 in _dbContext.Net_DevicePorts on x.LastDevicePortId equals z2.Id
                        into sub_Net_DevicePorts2
                        from zz2 in sub_Net_DevicePorts2.DefaultIfEmpty()
                        select new NetworkLinkList
                        {
                            Id = x.Id,
                            SerialNumber = x.SerialNumber,
                            Distance = x.Distance,
                            HeadDevice = yy1 != null ? yy1.Name : null,
                            LastDevice = yy2 != null ? yy1.Name : null,
                            HeadDevicePort = zz1 != null ? zz1.Name : null,
                            LastDevicePort = zz2 != null ? zz2.Name : null,
                            Type = x.Type,
                            ConnectType = x.ConnectType,
                            Speed = x.Speed,
                            Status = x.Status,
                            Note = x.Note
                        };
            return await query.ToListAsync();
        }

        public async Task<bool> IsDupicateAttributesAsync(Guid? Id, string Code)
        {
            bool result = false;
            if (string.IsNullOrEmpty(Code))
            {
                throw new Exception(Sys_Const.Message.SERVICE_CODE_NOT_EMPTY);
            }
            if (GuidHelpers.IsNullOrEmpty(Id))
            {
                result = await _dbContext.Net_NetworkLinks.Where(o => o.SerialNumber == Code).AnyAsync();
            }
            else
            {
                var count = await _dbContext.Net_NetworkLinks.Where(o => o.Id == Id && o.SerialNumber == Code).CountAsync();
                if (count <= 1)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return await Task.FromResult(result);
        }
    }
}
