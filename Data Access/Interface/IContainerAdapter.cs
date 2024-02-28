using Microsoft.Azure.Cosmos;

namespace StudentAPI.Interface
{
    public interface IContainerAdapter
    {
        /// <summary>
        /// Create database if not exists already
        /// </summary>
        /// <returns></returns>
        Task CreateDatabaseIfNotExistsAsync();

        /// <summary>
        /// Create container if not exists already
        /// </summary>
        /// <param name="containerId">ContainerId</param>
        /// <param name="partitionKeyPath">Partition key path of the container</param>
        /// <returns></returns>
        Task CreateContainerIfNotExistsAsync(string containerId, string partitionKeyPath);

        /// <summary>
        /// Get Container
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        Container GetContainer(string containerId);
    }
}
