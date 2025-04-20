using System.Linq.Expressions;
using RO.DevTest.Application.Features;

namespace RO.DevTest.Application.Contracts.Persistance.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations on entities of type <typeparamref name="T"/>.
/// This interface provides methods for creating, reading, updating, and deleting entities.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseRepository<T> where T : class 
{
    /// <summary>
    /// Retrieves a paginated list of entities from the database.
    /// </summary>
    /// <param name="pagination">The pagination details including page number and size.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A paginated result containing the entities and pagination metadata.</returns>
    Task<PaginatedResult<T>> GetAllPagedAsync(PaginationQuery pagination, CancellationToken cancellationToken = default);

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
    /// <param name="entity"> The entity to be created </param>
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
