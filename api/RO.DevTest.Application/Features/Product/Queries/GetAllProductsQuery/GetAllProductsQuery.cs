namespace RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;

using MediatR;
using RO.DevTest.Application.Features.User.Queries;

/// <summary>
/// Query to retrieve all products with pagination.
/// </summary>
/// <remarks>
/// This query accepts a <see cref="PaginationQuery"/> object to define pagination parameters such as page number,
/// page size, sorting options, and the sort order. It returns a <see cref="PaginatedResult{T}"/> containing a list
/// of products and their associated metadata (e.g., total count and total pages).
/// </remarks>
public class GetAllProductsQuery(PaginationQuery paginationQuery) : IRequest<PaginatedResult<Domain.Entities.Product>> { 
    public PaginationQuery PaginationQuery { get; set; } = paginationQuery;
}
