namespace kafkaclient.web.Core.Interfaces;

public interface ICreateOnlyRepository<T> where T: class
{
    ValueTask<T> CreateAsync(T entity);
    ValueTask<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities);
}