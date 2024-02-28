using Microsoft.Azure.Cosmos;
using StudentAPI.Interface;
using StudentAPI.Model;
using Container = Microsoft.Azure.Cosmos.Container;

namespace StudentAPI.Adapter
{
    /// <summary>
    /// Contains logic related to creation of database and containers
    /// </summary>
    public class ContainerAdapter : IContainerAdapter
    {
        private CosmosClient _client;

        private CosmosClientOptions _options;

        private string _databaseId;
        public ContainerAdapter(DbConfiguration dbConfiguration) 
        {
            _options = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Direct,
                GatewayModeMaxConnectionLimit = dbConfiguration.MaxConnectionLimit,
                RequestTimeout = TimeSpan.FromSeconds(dbConfiguration.RequestTimeout),
            };
            _databaseId = dbConfiguration.DatabaseId;
            _client = new CosmosClient(dbConfiguration.Endpoint, dbConfiguration.PrimaryKeySecret, _options);

        }

        /// <summary>
        /// Create container if not exists already
        /// </summary>
        /// <param name="containerId">ContainerId</param>
        /// <param name="partitionKeyPath">Partition key path of the container</param>
        /// <returns></returns>
        public async Task CreateContainerIfNotExistsAsync(string containerId, string partitionKeyPath)
        {
            await _client.GetDatabase(_databaseId).CreateContainerIfNotExistsAsync(containerId, partitionKeyPath).ConfigureAwait(false);
        }

        /// <summary>
        /// Create database if not exists already
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _client.CreateDatabaseIfNotExistsAsync(_databaseId).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Get Container details
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Container GetContainer(string containerId) =>
            _client.GetContainer(_databaseId, containerId);
    }
}
