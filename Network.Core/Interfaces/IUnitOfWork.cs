using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Network.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void CreateTransaction();
        void Commit();
        void Roolback();
        Task<int> SaveAsync(CancellationToken cancellationToken = default);        
    }
}
