using System.Linq.Expressions;
using System.Reflection;
using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Interfaces;
using kafkaclient.web.Core.Utils;
using kafkaclient.web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Infrastructure.Respository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    
    public GenericRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async ValueTask<T> CreateAsync(T entity)
    {   
        await _db.AddAsync(entity);
        int affectedRows = await _db.SaveChangesAsync();
        return entity;
    }

    public async ValueTask<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
        int affectedRows = await _db.SaveChangesAsync();
        return entities;
    }

    public async ValueTask<int> UpdateAsync(T entity)
    {
        _db.Entry(entity).State = EntityState.Modified;
        var CreatedAtProperty = typeof(T).GetProperty("CreatedAt");
        if (CreatedAtProperty != null)
        {
            _db.Entry(entity).Property("CreatedAt").IsModified = false;
        }

        int affectedRows = await _db.SaveChangesAsync();
        return affectedRows;
    }

    public async ValueTask<int> UpdateRangeAsync(IEnumerable<T> entities)
    {
        _db.Set<T>().UpdateRange(entities);
        int affectedRows = await _db.SaveChangesAsync();
        return affectedRows;
    }

    public async ValueTask<int> DeleteAsync(T entity)
    {
        _db.Set<T>().Remove(entity);
        int affectedRows = await _db.SaveChangesAsync();
        return affectedRows;
    }

    public async ValueTask<int> DeleteRangeAsync(IEnumerable<T> entities)
    {
        _db.Set<T>().RemoveRange(entities);
        int affectedRows = await _db.SaveChangesAsync();
        return affectedRows;
    }

    public async ValueTask<bool> ExistAnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _db.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public async ValueTask<PageResponse<T>> GetAllAsync<TFilter, TOrderBy>(
        int offset, int limit, 
        TFilter filter, TOrderBy orderBy,
        Dictionary<string, string> operators,
        List<FilterGroup> filterGroups = null,
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        var result = new PageResponse<T>();
        int count = 0;
        IQueryable<T> query = _db.Set<T>();

        if(include != null)
        {
            query = include(query);
        }  

        var expression = ExpressionFilterAndOrder.BuildLogicFilterGeneric<T, TFilter>(filterGroups, filter, operators);
        if(expression != null)
        {
            query = query.Where(expression);
        }

        PropertyInfo[] propertiesOrderby = typeof(TOrderBy).GetProperties();
        foreach (var item in propertiesOrderby)
        {
            var value = item.GetValue(orderBy);
            IOrderedQueryable<T> orderedQuery = null;
            if(value != null)
            {
                query = ExpressionFilterAndOrder.ApplyOrdering<T>(item.Name, (value as string), query, orderedQuery);
                orderedQuery = (IOrderedQueryable<T>)query;
            }
        }

        count = await query.CountAsync();
        query = query.Skip(offset - 1 * limit).Take(limit);

        result.Offset = offset;
        result.Limit = limit;  
        result.Total = count;
        result.Data = query.ToList();
        result.Count = result.Data.Count;

        return result;
    }

    public async ValueTask<T> GetByIdAsync(
        object id,
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> query = _db.Set<T>();

        if(include != null)
        {
            query = include(query);
        }

        var expression = ExpressionFilterAndOrder.BuildFilterByUnique<T>("Id", id, "=");
        if(expression != null)
        {
            query = query.Where(expression);
        }

        var result = await query.ToListAsync();
        return result.FirstOrDefault();
    }
}