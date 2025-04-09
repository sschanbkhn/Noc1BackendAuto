using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Core.Interfaces
{
    public interface IUserProvider
    {
        bool IsAuthenticated { get; }        
        Guid Id { get; }
        string UserName { get; }
    }
}
