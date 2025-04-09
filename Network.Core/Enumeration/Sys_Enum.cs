using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Enumeration
{
    public static class Sys_Enum
    {
        public enum StatusCode
        {
            Success = 200,
            Warning = 300,
            UnAuthorize = 401,
            NotFound = 404,
            InternalError = 500,
            TokenExpired = 600
        }
        public enum AuditType
        {
            None = 0,
            Create = 1,
            Update = 2,
            Delete = 3
        }
        public enum NotificationType
        {
            Success = 0,
            Warning = 1,
            Error = 2,
            Info = 3
        }
    }
}
