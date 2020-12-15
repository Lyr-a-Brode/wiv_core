using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace WivCore.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ILogger<HealthCheck> _logger;
        
        public HealthCheck(ILogger<HealthCheck> logger)
        {
            _logger = logger;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            using (LogContext.PushProperty("origin", ToString()))
            {
                _logger.LogInformation("Executing health check");    
            }
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}