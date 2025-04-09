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
using SysAccountVNPT.API.ViewModel.DsAccountDevice;
using Network.API.ViewModel.Net_Devices;

namespace Network.API.Service.Net_Devices
{
    public class Service : RepositoryBase<Model.Net_Devices>, Net_Devices.IService
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

        public async Task<List<DevicesList>> GetListAsync(Guid organId, int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_Devices
                        join y in _dbContext.Net_DeviceTypes on x.DeviceTypeId equals y.Id
                        into sub_Net_DeviceTypes
                        from yy in sub_Net_DeviceTypes.DefaultIfEmpty()
                        join z in _dbContext.Net_Manufacturers on x.ManufacturerId equals z.Id
                        into sub_Net_Manufacturers
                        from zz in sub_Net_Manufacturers.DefaultIfEmpty()
                        join o in _dbContext.Sys_Organizations on x.OrganId equals o.Id
                        into sub_Sys_Organizations
                        from oo in sub_Sys_Organizations.DefaultIfEmpty()
                        where x.OrganId == organId
                        select new DevicesList { 
                            Id = x.Id, Code = x.Code, Name = x.Name, Lon_Lat = x.Lon + "/" + x.Lat, Description = x.Description,
                            DeviceType = yy != null ? yy.Name : null,
                            Manufacturer = zz != null ? zz.Name : null,
                            FirmwareVersion = x.FirmwareVersion,
                            IPAddress = x.IPAddress,
                            MACAddress = x.MACAddress,
                            SerialNumber = x.SerialNumber,
                            Organ = oo != null ? oo.Name : null
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
                result = await _dbContext.Net_Devices.Where(o => o.Code == Code).AnyAsync();
            }
            else
            {
                var count = await _dbContext.Net_Devices.Where(o => o.Id == Id && o.Code == Code).CountAsync();
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

        public async Task<List<SwitchesResult>> GetSwitchesAsync()
        {
            var items = from x in _dbContext.Net_Devices
                        select new SwitchesResult
                        {
                            id = x.Id.ToString(),
                            name = x.Name,
                            lat = x.Lat,
                            lon = x.Lon,
                            active = true
                        };
            return await items.ToListAsync();
        }
    }
}
