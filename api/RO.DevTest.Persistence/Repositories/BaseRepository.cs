namespace RO.DevTest.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using System.Linq.Expressions;
using Application.Features;

public class BaseRepository<T>(DefaultContext defaultContext) : IBaseRepository<T> where T : class 
{
    protected DefaultContext Context { get => defaultContext; }

    public async Task<PaginatedResult<T>> GetAllPagedAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<T>().AsQueryable();

        if (!string.IsNullOrEmpty(paginationQuery.SortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, paginationQuery.SortBy);
            var lambda = Expression.Lambda(property, parameter);

            query = paginationQuery.isAscend
                ? Queryable.OrderBy(query, (dynamic)lambda)
                : Queryable.OrderByDescending(query, (dynamic)lambda);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                                .Take(paginationQuery.PageSize)
                                .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalCount, paginationQuery.PageNumber, paginationQuery.PageSize);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await Context.Set<T>().ToListAsync(cancellationToken)).AsEnumerable();
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default) 
    {
        await Context.Set<T>().AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity) 
    {
        Context.Set<T>().Update(entity);
        return await Context.SaveChangesAsync().ContinueWith(t => t.Result > 0 ? entity : null);
    }

    public async Task<bool> DeleteAsync(T entity) 
    {
        Context.Set<T>().Remove(entity);
        return await Context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Generates a filtered <see cref="IQueryable{T}"/>, based on its
    /// <paramref name="predicate"/> and <paramref name="includes"/>, including
    /// the data requested
    /// </summary>
    /// <param name="predicate">
    /// The <see cref="Expression"/> to use as filter
    /// </param>
    /// <param name="includes">
    /// The <see cref="Expression"/> to use as include
    /// </param>
    /// <returns>
    /// The generated <see cref="IQueryable{T}"/>
    /// </returns>
    private IQueryable<T> GetQueryWithIncludes(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes
    ) 
    {
        IQueryable<T> baseQuery = GetWhereQuery(predicate);

        foreach(Expression<Func<T, object>> include in includes) {
            baseQuery = baseQuery.Include(include);
        }

        return baseQuery;
    }

    /// <summary>
    /// Generates an <see cref="IQueryable"/> based on
    /// the <paramref name="predicate"/>
    /// </summary>
    /// <param name="predicate">
    /// An <see cref="Expression"/> representing a filter
    /// of it
    /// </param>
    /// <returns>S
    /// The <see cref="IQueryable{T}"/>
    /// </returns>
    private IQueryable<T> GetWhereQuery(Expression<Func<T, bool>> predicate) 
    {
        IQueryable<T> baseQuery = Context.Set<T>();

        if(predicate is not null) {
            baseQuery = baseQuery.Where(predicate);
        }

        return baseQuery;
    }
}
