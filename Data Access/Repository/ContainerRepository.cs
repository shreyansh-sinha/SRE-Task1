using Microsoft.Azure.Cosmos;
using StudentAPI.Data_Access.Interface;
using StudentAPI.Interface;
using StudentAPI.Model;
using System.Linq.Expressions;
using Container = Microsoft.Azure.Cosmos.Container;

namespace StudentAPI.Repository
{
    /// <summary>
    /// Class contains methods to Add, Delete, Update, Create entries in container of database
    /// </summary>
    /// <param name="containerAdapter"></param>
    /// <param name="linqWrapper"></param>
    /// <param name="containerId"></param>
    public class ContainerRepository(IContainerAdapter containerAdapter, ILinqWrapper linqWrapper, string containerId) : IContainerRepository<Student>
    {
        private Container _container = containerAdapter.GetContainer(containerId);
        private readonly ILinqWrapper myLinqWrapper = linqWrapper;
        public async Task<Student> AddAsync(Student entity)
        {
            var document = await _container.CreateItemAsync(entity).ConfigureAwait(false);

            return document;
        }

        public async Task DeleteAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<Student>(id, new PartitionKey(partitionKey));
        }

        public async Task<Student> GetItemAsync(string id, string partitionKey)
        {
            ItemResponse<Student> item = await _container.ReadItemAsync<Student>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
            return item.Resource;
        }

        public async Task<Student> UpdateAsync(Student entity)
        {
            ItemResponse<Student> item = await _container.ReplaceItemAsync(entity, entity.Id).ConfigureAwait(false);
            return item.Resource;
        }

        public async Task<IEnumerable<Student>> FilterAsync(Expression<Func<Student, bool>> filter)
        {
            IQueryable<Student> query = _container.GetItemLinqQueryable<Student>(requestOptions: new QueryRequestOptions { MaxItemCount = 10 })
                                    .Where(filter);
            return await Iterator(query).ConfigureAwait(false);
        }

        private async Task<IEnumerable<Student>> Iterator(IQueryable<Student> query)
        {
            List<Student> documents = [];
            using (FeedIterator<Student> queryable = myLinqWrapper.GetFeedIterator(query))
            {
                while (queryable.HasMoreResults)
                {
                    FeedResponse<Student> docs = await queryable.ReadNextAsync().ConfigureAwait(false);
                    documents.AddRange(docs);
                }
            }
            return documents;
        }
    }
}
