using Microsoft.Azure.Cosmos;

namespace StudentAPI.Data_Access.Interface
{
    public interface ILinqWrapper
    {
        FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query);
    }
}
