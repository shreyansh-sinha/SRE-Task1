using StudentAPI.Model;
using System.Linq.Expressions;

namespace StudentAPI.Interface
{
    public interface IContainerRepository<T> where T : class
    {
        /// <summary>
        /// Insert async.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A Task.</returns>
        Task<Student> UpdateAsync(T entity);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <param name="partitionKey">Partition key</param>
        /// <returns>A Task.</returns>
        Task DeleteAsync(string id, string partitionKey);

        /// <summary>
        /// Get an Item, using id and Partition key
        /// </summary>
        /// <param name="id">Id value</param>
        /// <param name="partitionKey">Partition key value</param>
        /// <returns></returns>
        Task<Student> GetItemAsync(string id, string partitionKey);

        Task<IEnumerable<Student>> FilterAsync(Expression<Func<Student, bool>> filter);
    }
}
