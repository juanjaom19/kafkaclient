namespace kafkaclient.web.Core.Interfaces;

public interface IDeleteOnlyRepository<T> where T : class
{
    ValueTask<int> DeleteAsync(T entity);
    ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities);
}