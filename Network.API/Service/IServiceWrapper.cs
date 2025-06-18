using Network.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service
{
    public interface IServiceWrapper
    {
        Sys_AuthToken.IService Sys_AuthToken { get; }
        Sys_File.IService Sys_File { get; }
        Sys_User.IService Sys_User { get; }
        Sys_Category.IService Sys_Category { get; }
        Sys_Organization.IService Sys_Organization { get; }
        Sys_Role.IService Sys_Role { get; }
        Sys_Config.IService Sys_Config { get; }
        Sys_Permission.IService Sys_Permission { get; }
        Sys_Resource.IService Sys_Resource { get; }
        Sys_Notification.IService Sys_Notification { get; }

        Net_AlarmType.IService Net_AlarmType { get; }
        Net_CableManagement.IService Net_CableManagement { get; }
        Net_ConfigurationLogs.IService Net_ConfigurationLogs { get; }
        Net_CurenAlarm.IService Net_CurenAlarm { get; }
        Net_DevicePorts.IService Net_DevicePorts { get; }
        Net_Devices.IService Net_Devices { get; }
        Net_DeviceTypes.IService Net_DeviceTypes { get; }
        Net_HistoryCurenAlarm.IService Net_HistoryCurenAlarm { get; }
        Net_Manufacturers.IService Net_Manufacturers { get; }
        Net_NetworkLinks.IService Net_NetworkLinks { get; }
        Net_UC_LinhVucs.IService Net_UC_LinhVucs { get; }
        Net_UC_TrangThais.IService Net_UC_TrangThais { get; }
        NetUsecase_Runs.IService NetUsecase_Runs { get; }
    }
}
