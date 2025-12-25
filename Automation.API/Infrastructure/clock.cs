using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Infrastructure
{
    public class clock : IClock
    {
        public DateTime Now => DateTime.Now.AddMinutes(5);

        public DateTime UtcNow => DateTime.UtcNow.AddMinutes(5);
    }
}
