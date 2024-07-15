namespace kafkaclient.web.Core.Interfaces;

public interface IUpdateOnlyRepository<T> where T: class
{
    ValueTask<int> UpdateAsync(T entity);
    ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities);
}