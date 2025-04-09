using Network.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Core
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTimeOffset OffsetNow => DateTimeOffset.Now;

        public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }
}
