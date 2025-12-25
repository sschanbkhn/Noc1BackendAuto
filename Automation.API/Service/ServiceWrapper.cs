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
        private readonly IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;
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
        private Speed_DataOkla.IService _speed_dataokla;
        private Speed_ThongTinNhanVienDo.IService _speed_thongtinnhanviendo;
        private Sys_EmailSms.IService _sys_emailsms;
        private Speed_SmsEmail.IService _speed_smsemail;
        private Speed_FileImport.IService _speed_fileimport;
        private Speed_Debian.IService _speed_debian;
        private Rnoc_R009.IService _rnoc_r009;
        private Rnoc_R001.IService _rnoc_r001;
        private Rnoc_R008.IService _rnoc_r008;
        private I004_1_LSP.IService _i004_1_lsp;
        private I004_LSP.IService _i004_lsp;
        private I003_BNG.IService _i003_bng;
        private I002_HardwareAlarm.IService _i002_hardwarealarm;
        public ServiceWrapper(DomainDbContext context, IDateTimeProvider dateTimeProvider, IUserProvider userService, IConfiguration configuration)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
            _configuration = configuration;
            _httpClientFactory = null; // Will be resolved when needed
            _loggerFactory = null; // Will be resolved when needed
        }
        //
        public Speed_FileImport.IService Speed_FileImport
        {
            get
            {
                if (_speed_fileimport == null)
                {
                    _speed_fileimport = new Speed_FileImport.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _speed_fileimport;
            }
        }
        public Sys_EmailSms.IService Sys_EmailSms
        {
            get
            {
                if (_sys_emailsms == null)
                {
                    _sys_emailsms = new Sys_EmailSms.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _sys_emailsms;
            }
        }
         public Speed_SmsEmail.IService Speed_SmsEmail
        {
            get
            {
                if (_speed_smsemail == null)
                {
                    _speed_smsemail = new Speed_SmsEmail.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _speed_smsemail;
            }
        }

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
        public Speed_DataOkla.IService Speed_DataOkla
        {
            get
            {
                if (_speed_dataokla == null)
                {
                    _speed_dataokla = new Speed_DataOkla.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _speed_dataokla;
            }
        }
        public Speed_ThongTinNhanVienDo.IService Speed_ThongTinNhanVienDo
        {
            get
            {
                if (_speed_thongtinnhanviendo == null)
                {
                    _speed_thongtinnhanviendo = new Speed_ThongTinNhanVienDo.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _speed_thongtinnhanviendo;
            }
        }
        public Speed_Debian.IService Speed_Debian
        {
            get
            {
                if (_speed_debian == null)
                {
                    _speed_debian = new Speed_Debian.Service(_context, _dateTimeProvider, _userProvider);
                }

                return _speed_debian;
            }
        }
        public Rnoc_R009.IService Rnoc_R009
        {
            get
            {
                if (_rnoc_r009 == null)
                {
                    _rnoc_r009 = new Rnoc_R009.Service(_configuration, _dateTimeProvider, _userProvider);
                }

                return _rnoc_r009;
            }
        }
        
        public Rnoc_R001.IService Rnoc_R001
        {
            get
            {
                if (_rnoc_r001 == null)
                {
                    _rnoc_r001 = new Rnoc_R001.Service(_configuration, _dateTimeProvider, _userProvider);
                }

                return _rnoc_r001;
            }
        }
        
        public Rnoc_R008.IService Rnoc_R008
        {
            get
            {
                if (_rnoc_r008 == null)
                {
                    // Create a null logger as R008 service can work without detailed logging
                    var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Rnoc_R008.Service>();
                    _rnoc_r008 = new Rnoc_R008.Service(_configuration, logger);
                }

                return _rnoc_r008;
            }
        }
        
        public I004_1_LSP.IService I004_1_LSP
        {
            get
            {
                if (_i004_1_lsp == null)
                {
                    _i004_1_lsp = new I004_1_LSP.Service(_context, _dateTimeProvider, _userProvider, _configuration);
                }

                return _i004_1_lsp;
            }
        }
        
        public I004_LSP.IService I004_LSP
        {
            get
            {
                if (_i004_lsp == null)
                {
                    _i004_lsp = new I004_LSP.Service(null, _configuration);
                }

                return _i004_lsp;
            }
        }
        
        public I003_BNG.IService I003_BNG
        {
            get
            {
                if (_i003_bng == null)
                {
                    // Create a simple logger if factory is null
                    Microsoft.Extensions.Logging.ILogger<I003_BNG.Service> logger = null;
                    if (_loggerFactory != null)
                    {
                        logger = _loggerFactory.CreateLogger("I003_BNG.Service") as Microsoft.Extensions.Logging.ILogger<I003_BNG.Service>;
                    }
                    
                    _i003_bng = new I003_BNG.Service(_context, _dateTimeProvider, _userProvider, _configuration, _httpClientFactory, logger);
                }

                return _i003_bng;
            }
        }
        
        public I002_HardwareAlarm.IService I002_HardwareAlarm
        {
            get
            {
                if (_i002_hardwarealarm == null)
                {
                    // Create a simple logger if factory is null
                    Microsoft.Extensions.Logging.ILogger<I002_HardwareAlarm.Service> logger = null;
                    if (_loggerFactory != null)
                    {
                        logger = _loggerFactory.CreateLogger("I002_HardwareAlarm.Service") as Microsoft.Extensions.Logging.ILogger<I002_HardwareAlarm.Service>;
                    }
                    
                    _i002_hardwarealarm = new I002_HardwareAlarm.Service(_context, _dateTimeProvider, _userProvider, _configuration, _httpClientFactory, logger);
                }

                return _i002_hardwarealarm;
            }
        }
    }
}
