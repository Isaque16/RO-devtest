namespace RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;

using RO.DevTest.Domain.Entities;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.User.Queries;
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
    // Busca os produtos paginados no banco de dados
    var (products, totalCount) = await productRepo.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

    // Mapeia os produtos para o resultado da query
    var result = products.Select(product => new Product
    {
      Id = product.Id,
      Name = product.Name ?? string.Empty,
      Description = product.Description ?? string.Empty,
      Price = product.Price,
    }).ToList();

    return new PaginatedResult<Product>(result, totalCount, request.PageNumber, request.PageSize);
  }
}
