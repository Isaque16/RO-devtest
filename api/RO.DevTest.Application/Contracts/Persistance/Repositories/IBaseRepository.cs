using System.Linq.Expressions;

namespace RO.DevTest.Application.Contracts.Persistance.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations on entities of type <typeparamref name="T"/>.
/// This interface provides methods for creating, reading, updating, and deleting entities,
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseRepository<T> where T : class 
{
    /// <summary>
    /// Retrieves all entities from the database in a paginated format
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A paginated list of entities</returns>
    Task<(IEnumerable<T> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities from the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of all entities</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds an entity by its identifier
    /// </summary>
    /// <param name="id">The identifier of the entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>
    /// The <typeparamref name="T"/> entity, if found. Null otherwise.
    /// </returns>
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity in the database
    /// </summary>
    /// <param name="entity"> The entity to be create </param>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> The created entity </returns>
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an entity entry on the database
    /// </summary>
    /// <param name="entity"> The entity to be added </param>
    Task<T?> UpdateAsync(T entity);

    /// <summary>
    /// Deletes one entry from the database
    /// </summary>
    /// <param name="entity"> The entity to be deleted </param>
    Task<bool> DeleteAsync(T entity);
}
