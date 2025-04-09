using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Models
{
    public class ResponseResult
    {
        public object Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; } = false;
    }
}
