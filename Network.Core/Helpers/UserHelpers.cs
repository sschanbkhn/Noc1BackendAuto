using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Core.Constant;

namespace Network.Core.Helpers
{
    public static class UserHelpers
    {
        public static bool IsValidUserLogin(string userName, string password, out string message)
        {
            message = "";
            if (string.IsNullOrWhiteSpace(userName))
            {
                message = Sys_Const.Message.SERVICE_LOGIN_USERNAME_EMPTY;
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                message = Sys_Const.Message.SERVICE_LOGIN_PASSWORD_EMPTY;
                return false;
            }
            return true;
        }
        public static bool IsValidUserChangePassword(string userName, string passwordOld, string passwordNew, out string message)
        {
            message = "";

            if (string.IsNullOrWhiteSpace(userName))
            {
                message = Sys_Const.Message.SERVICE_LOGIN_USERNAME_EMPTY;
                return false;
            }
            if (string.IsNullOrEmpty(passwordOld))
            {
                message = Sys_Const.Message.SERVICE_LOGIN_PASSWORDOld_EMPTY;
                return false;
            }

            if (string.IsNullOrWhiteSpace(passwordNew))
            {
                message = Sys_Const.Message.SERVICE_LOGIN_PASSWORDNEW_EMPTY;
                return false;
            }

            if (passwordOld == passwordNew)
            {
                message = Sys_Const.Message.SERVICE_LOGIN_PASSNEW_PASSOld_DIFFERENT;
                return false;
            }
            return true;
        }
    }
}
