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
        Speed_DataOkla.IService Speed_DataOkla { get; }
        Speed_ThongTinNhanVienDo.IService Speed_ThongTinNhanVienDo { get; }
        Speed_FileImport.IService Speed_FileImport { get; }
        Sys_EmailSms.IService Sys_EmailSms { get; }
        Speed_SmsEmail.IService Speed_SmsEmail { get; }
        Speed_Debian.IService Speed_Debian { get; }
        Rnoc_R009.IService Rnoc_R009 { get; }
        Rnoc_R001.IService Rnoc_R001 { get; }
        Rnoc_R008.IService Rnoc_R008 { get; }
        I004_1_LSP.IService I004_1_LSP { get; }
        I004_LSP.IService I004_LSP { get; }
        I003_BNG.IService I003_BNG { get; }
        I002_HardwareAlarm.IService I002_HardwareAlarm { get; }
    }
}
