namespace RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using Domain.Entities;

/// <summary>
/// Handles the query to get a product by its ID.
/// </summary>
/// <param name="productRepository"></param>
public class GetProductByIdQueryHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, Product>
{
  /// <summary>
  /// Handles the request to get a product by its ID.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns>
  ///  Returns the product with the specified ID.
  /// </returns>
  /// <exception cref="KeyNotFoundException"></exception>
  public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
  {
    return await productRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Produto com ID {request.Id} não foi encontrado.");
  }
}
