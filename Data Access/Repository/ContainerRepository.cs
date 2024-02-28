using Microsoft.Azure.Cosmos;
using StudentAPI.Interface;
using StudentAPI.Model;
using Container = Microsoft.Azure.Cosmos.Container;

namespace StudentAPI.Repository
{
    /// <summary>
    /// Class contains methods to Add, Delete, Update, Create entries in container of database
    /// </summary>
    /// <param name="containerAdapter"></param>
    /// <param name="containerId"></param>
    public class ContainerRepository(IContainerAdapter containerAdapter, string containerId) : IContainerRepository<Student>
    {
        private Container _container = containerAdapter.GetContainer(containerId);
        public async Task<Student> AddAsync(Student entity)
        {
            var document = await _container.CreateItemAsync(entity).ConfigureAwait(false);

            return document;
        }

        public async Task DeleteAsync(string id, string partitionKey)
        {
            var res = await _container.DeleteItemAsync<Student>(id, new PartitionKey(partitionKey));
        }

        public async Task<Student> GetItemAsync(string id, string partitionKey)
        {
            ItemResponse<Student> item = await _container.ReadItemAsync<Student>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
            return item.Resource;
        }

        public async Task UpdateAsync(Student entity)
        {
            await _container.ReplaceItemAsync(entity, entity.Id).ConfigureAwait(false);
        }
    }
}
