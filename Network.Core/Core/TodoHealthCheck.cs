using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.Core.Core
{
    public class TodoHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //Implement you logic here    
            var healthy = true;
            if (healthy)
                return Task.FromResult(HealthCheckResult.Healthy());
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}
