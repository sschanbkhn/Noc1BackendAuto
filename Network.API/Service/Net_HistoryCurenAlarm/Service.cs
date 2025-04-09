using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.API.ViewModel.Net_CurenAlarm;
using Network.API.ViewModel.Net_HistoryCurenAlarm;
using Network.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Net_HistoryCurenAlarm
{
    public class Service : RepositoryBase<Model.Net_HistoryCurenAlarm>, Net_HistoryCurenAlarm.IService
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

        public async Task<List<HistoryCurenAlarmList>> GetListAsync(int page, int pageSize, int totalLimitItems)
        {
            var query = from x in _dbContext.Net_HistoryCurenAlarm
                        join a in _dbContext.Net_Devices on x.DeviceId equals a.Id
                        into sub_Net_Devices
                        from aa in sub_Net_Devices.DefaultIfEmpty()
                        join b in _dbContext.Net_AlarmType on x.AlarmTypeId equals b.Id
                        into sub_Net_AlarmType
                        from bb in sub_Net_AlarmType.DefaultIfEmpty()
                        join c in _dbContext.Sys_Categories.Where(o => o.Type == Core.Enums.CategoryType.AlarmCode) on x.AlarmCode equals c.Code
                        into sub_Sys_Categories_AlarmCode
                        from cc in sub_Sys_Categories_AlarmCode.DefaultIfEmpty()
                        join d in _dbContext.Sys_Categories.Where(o => o.Type == Core.Enums.CategoryType.Status) on x.Status equals d.Id
                        into sub_Sys_Categories_Status
                        from dd in sub_Sys_Categories_Status.DefaultIfEmpty()
                        join e in _dbContext.Sys_Categories.Where(o => o.Type == Core.Enums.CategoryType.AlarmLevel) on x.LevelId equals e.Id
                        into sub_Sys_Categories_AlarmLevel
                        from ee in sub_Sys_Categories_AlarmLevel.DefaultIfEmpty()
                        select new HistoryCurenAlarmList
                        {
                            Id = x.Id,
                            AlarmType = bb != null ? bb.Name : null,
                            Device = aa != null ? aa.Name : null,
                            Level = ee != null ? ee.Name : null,
                            Status = dd != null ? dd.Name : null,
                            IncidentTime = x.IncidentTime != null ? x.IncidentTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : null,
                            RecoveryTime = x.RecoveryTime != null ? x.RecoveryTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : null,
                            AlarmDetail = x.AlarmDetail,
                            AlarmCode = x.AlarmCode,
                            Reason = x.Reason,
                            Note = x.Note,
                            ProcessedContent = x.ProcessedContent
                        };
            return await query.ToListAsync();
        }
    }
}
