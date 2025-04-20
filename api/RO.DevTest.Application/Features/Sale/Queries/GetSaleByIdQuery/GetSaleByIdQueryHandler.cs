namespace RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;

using MediatR;
using Contracts.Persistance.Repositories;
using Domain.Entities;

/// <summary>
/// Handles the execution of the GetSaleByIdQuery, which is responsible for retrieving a <see cref="Sale"/> entity
/// based on the specified sale ID.
/// </summary>
/// <remarks>
/// This handler processes the query by utilizing an implementation of <see cref="ISaleRepository"/> to access the data.
/// If no sale is found for the given ID, a <see cref="KeyNotFoundException"/> is thrown.
/// </remarks>
public class GetSaleByIdQueryHandler(ISaleRepository saleRepo) : IRequestHandler<GetSaleByIdQuery, Sale>
{
  /// <summary>
  /// Processes the GetSaleByIdQuery to retrieve the Sale entity corresponding to the specified sale ID.
  /// </summary>
  /// <param name="request">The query object containing the sale ID to be retrieved.</param>
  /// <param name="cancellationToken">A token that allows the operation to be cancelled.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the Sale entity if found.</returns>
  /// <exception cref="KeyNotFoundException">Thrown when no Sale entity is found for the given sale ID.</exception>
  public async Task<Sale> Handle(GetSaleByIdQuery request,
    CancellationToken cancellationToken)
  {
    return await saleRepo.GetByIdAsync(request.Id, cancellationToken) ??
           throw new KeyNotFoundException($"Venda com ID {request.Id} não foi encontrada.");
  }
}
