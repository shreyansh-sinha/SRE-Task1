using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace StudentAPI.Util
{
    public static class HealthCheckServiceExtension
    {
        // logic to map health check endpoint
        public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // here a endpoint is configured for the health check ping. it can be accessed 
            // using <baseUrl>/ping.
            endpoints.MapHealthChecks("/ping", new HealthCheckOptions
            {
                // out of all health check configured, use only those health check which has tag "ready"
                Predicate = (check) => check.Tags.Contains("ready"),
                // result contains the health check report, use the status value to return the health check result
                ResponseWriter = (context, result) => context.Response.WriteAsync(result.Status.ToString())
            });

            endpoints.MapHealthChecks("/cosmosDb", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("db-check"),
                ResponseWriter = (context, result) => context.Response.WriteAsync(result.Status.ToString())
            });
        }
    }
}
