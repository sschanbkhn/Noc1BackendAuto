using Microsoft.AspNetCore.Mvc;
using Network.Core.Constant;
using Network.Core.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public static class ResponseMessage
    {
        private static OkObjectResult ObjectResult (object data, string message, bool success, int statusCode)
        {
            ResponseResult result = new ResponseResult();
            result.Data = data;
            result.StatusCode = statusCode;
            result.Success = success;
            result.Message = message;
            return new OkObjectResult(result);
        }
        public static OkObjectResult Success(object data = null, string message = Sys_Const.Message.SERVICE_SUCCESS, int statusCode = (int)Sys_Enum.StatusCode.Success)
        {
            return ObjectResult(data, message, true, statusCode);
        }
        public static OkObjectResult Success(object data = null)
        {
            return ObjectResult(data, Sys_Const.Message.SERVICE_SUCCESS, true, (int)Sys_Enum.StatusCode.Success);            
        }
        public static OkObjectResult Success()
        {
            return ObjectResult(null, Sys_Const.Message.SERVICE_SUCCESS, true, (int)Sys_Enum.StatusCode.Success);
        }
        public static OkObjectResult Error(string Message, object data, int statusCode)
        {
            return ObjectResult(null, Message, false, statusCode);
        }
        public static OkObjectResult Error(string Message)
        {
            return ObjectResult(null, Message, false, (int)Sys_Enum.StatusCode.InternalError);
        }
    }
}
