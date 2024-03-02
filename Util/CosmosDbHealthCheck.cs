using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StudentAPI.Interface;

public class CosmosDbHealthCheck : IHealthCheck
{
    private readonly IContainerAdapter _containerAdapter;
    private readonly CosmosClient _cosmosClient;

    public CosmosDbHealthCheck(IContainerAdapter containerAdapter)
    {
        _containerAdapter = containerAdapter ?? throw new ArgumentNullException(nameof(containerAdapter));
        _cosmosClient = _containerAdapter.GetCosmosClient();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Attempt to query Cosmos DB to verify connectivity
            await _cosmosClient.ReadAccountAsync();

            // Cosmos DB connection is healthy
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            // Cosmos DB connection is unhealthy
            return HealthCheckResult.Unhealthy("Failed to connect to Cosmos DB", ex);
        }
    }
}
