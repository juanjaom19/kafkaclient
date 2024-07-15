using System.Linq.Expressions;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Core.Interfaces;

public interface IReadOnlyRepository<T> where T : class
{
    ValueTask<PageResponse<T>> GetAllAsync<TFilter, TOrderBy>(
        int offset, int limit, 
        TFilter filter, TOrderBy orderBy,
        Dictionary<string, string> operators,
        List<FilterGroup> filterGroups = null,
        Func<IQueryable<T>, IQueryable<T>> include = null);
    ValueTask<T> GetByIdAsync(object id, Func<IQueryable<T>, IQueryable<T>> include = null);
    ValueTask<bool> ExistAnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}