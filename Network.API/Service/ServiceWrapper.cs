using Microsoft.Extensions.Configuration;
using Network.API.Infrastructure;
using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Network.API.Model;

namespace Network.API.Service
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly DomainDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        private Sys_AuthToken.IService _sys_authtoken;
        private Sys_Category.IService _sys_category;
        private Sys_User.IService _sys_user;
        private Sys_Role.IService _sys_role;
        private Sys_Config.IService _sys_config;
        private Sys_Resource.IService _sys_resource;
        private Sys_Organization.IService _sys_organization;
        private Sys_Permission.IService _sys_permission;
        private Sys_File.IService _sys_file;
        private Sys_Notification.IService _sys_notification;

        private Net_AlarmType.IService _net_alarmtype;
        private Net_CableManagement.IService _net_cablemanagement;
        private Net_ConfigurationLogs.IService _net_configurationlogs;
        private Net_CurenAlarm.IService _net_curenalarm;
        private Net_DevicePorts.IService _net_deviceports;
        private Net_Devices.IService _net_devices;
        private Net_DeviceTypes.IService _net_devicetypes;
        private Net_HistoryCurenAlarm.IService _net_historycurenalarm;
        private Net_Manufacturers.IService _net_manufacturers;
        private Net_NetworkLinks.IService _net_networklinks;
        private Net_UC_LinhVucs.IService _net_uc_linhvuc;
        private Net_UC_TrangThais.IService _net_uc_trangthai;
        private NetUsecase_Runs.IService _netusecase_run;

        public ServiceWrapper(DomainDbContext context, IDateTimeProvider dateTimeProvider, IUserProvider userService, IConfiguration configuration)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }
        //
        public Sys_Notification.IService Sys_Notification
        {
            get
            {
                if (_sys_notification == null)
                {
                    _sys_notification = new Sys_Notification.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_notification;
            }
        }
        public Sys_AuthToken.IService Sys_AuthToken
        {
            get
            {
                if (_sys_authtoken == null)
                {
                    _sys_authtoken = new Sys_AuthToken.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_authtoken;
            }
        }
        public Sys_File.IService Sys_File
        {
            get
            {
                if (_sys_file == null)
                {
                    _sys_file = new Sys_File.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_file;
            }
        }
        public Sys_Category.IService Sys_Category
        {
            get
            {
                if (_sys_category == null)
                {
                    _sys_category = new Sys_Category.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_category;
            }
        }
        public Sys_User.IService Sys_User
        {
            get
            {
                if (_sys_user == null)
                {
                    _sys_user = new Sys_User.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_user;
            }
        }
        public Sys_Organization.IService Sys_Organization
        {
            get
            {
                if (_sys_organization == null)
                {
                    _sys_organization = new Sys_Organization.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_organization;
            }
        }
        public Sys_Role.IService Sys_Role
        {
            get
            {
                if (_sys_role == null)
                {
                    _sys_role = new Sys_Role.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_role;
            }
        }
        public Sys_Config.IService Sys_Config
        {
            get
            {
                if (_sys_config == null)
                {
                    _sys_config = new Sys_Config.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_config;
            }
        }
        public Sys_Resource.IService Sys_Resource
        {
            get
            {
                if (_sys_resource == null)
                {
                    _sys_resource = new Sys_Resource.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_resource;
            }
        }
        public Sys_Permission.IService Sys_Permission
        {
            get
            {
                if (_sys_permission == null)
                {
                    _sys_permission = new Sys_Permission.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_permission;
            }
        }

        public Net_AlarmType.IService Net_AlarmType
        {
            get
            {
                if (_net_alarmtype == null)
                {
                    _net_alarmtype = new Net_AlarmType.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_alarmtype;
            }
        }

        public Net_CableManagement.IService Net_CableManagement
        {
            get
            {
                if (_net_cablemanagement == null)
                {
                    _net_cablemanagement = new Net_CableManagement.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_cablemanagement;
            }
        }

        public Net_ConfigurationLogs.IService Net_ConfigurationLogs
        {
            get
            {
                if (_net_configurationlogs == null)
                {
                    _net_configurationlogs = new Net_ConfigurationLogs.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_configurationlogs;
            }
        }

        public Net_CurenAlarm.IService Net_CurenAlarm
        {
            get
            {
                if (_net_curenalarm == null)
                {
                    _net_curenalarm = new Net_CurenAlarm.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_curenalarm;
            }
        }

        public Net_DevicePorts.IService Net_DevicePorts
        {
            get
            {
                if (_net_deviceports == null)
                {
                    _net_deviceports = new Net_DevicePorts.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_deviceports;
            }
        }

        public Net_Devices.IService Net_Devices
        {
            get
            {
                if (_net_devices == null)
                {
                    _net_devices = new Net_Devices.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_devices;
            }
        }

        public Net_DeviceTypes.IService Net_DeviceTypes
        {
            get
            {
                if (_net_devicetypes == null)
                {
                    _net_devicetypes = new Net_DeviceTypes.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_devicetypes;
            }
        }

        public Net_HistoryCurenAlarm.IService Net_HistoryCurenAlarm
        {
            get
            {
                if (_net_historycurenalarm == null)
                {
                    _net_historycurenalarm = new Net_HistoryCurenAlarm.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_historycurenalarm;
            }
        }

        public Net_Manufacturers.IService Net_Manufacturers
        {
            get
            {
                if (_net_manufacturers == null)
                {
                    _net_manufacturers = new Net_Manufacturers.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_manufacturers;
            }
        }

        public Net_NetworkLinks.IService Net_NetworkLinks
        {
            get
            {
                if (_net_networklinks == null)
                {
                    _net_networklinks = new Net_NetworkLinks.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_networklinks;
            }
        }
        public Net_UC_LinhVucs.IService Net_UC_LinhVucs
        {
            get
            {
                if (_net_uc_linhvuc == null)
                {
                    _net_uc_linhvuc = new Net_UC_LinhVucs.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_uc_linhvuc;
            }
        }
        public Net_UC_TrangThais.IService Net_UC_TrangThais
        {
            get
            {
                if (_net_uc_trangthai == null)
                {
                    _net_uc_trangthai = new Net_UC_TrangThais.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _net_uc_trangthai;
            }
        }
        public NetUsecase_Runs.IService NetUsecase_Runs
        {
            get
            {
                if (_netusecase_run == null)
                {
                    _netusecase_run = new NetUsecase_Runs.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _netusecase_run;
            }
        }
        
    }
}
