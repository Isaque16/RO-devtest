namespace RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;

using Domain.Entities;
using Contracts.Persistance.Repositories;
using MediatR;

/// <summary>
/// Handles the GetAllProductsQuery and retrieves a paginated list of products from the repository.
/// </summary>
/// <param name="productRepo"></param>
public class GetAllProductsQueryHandler(IProductRepository productRepo) : IRequestHandler<GetAllProductsQuery, PaginatedResult<Product>>
{
  /// <summary>
  /// Handles the GetAllProductsQuery and retrieves a paginated list of products from the repository.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns>
  ///  Returns a paginated result of products.
  /// </returns>
  public async Task<PaginatedResult<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
  {
    var products = await productRepo.GetAllPagedAsync(request.PaginationQuery, cancellationToken);
    return new PaginatedResult<Product>(products.Content, products.TotalCount, products.PageNumber, products.PageSize);
  }
}
