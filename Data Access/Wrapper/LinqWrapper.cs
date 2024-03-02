using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using StudentAPI.Data_Access.Interface;

namespace StudentAPI.Data_Access.Wrapper
{
    public class LinqWrapper : ILinqWrapper
    {
        public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query)
        {
            return query.ToFeedIterator();
        }
    }
}
