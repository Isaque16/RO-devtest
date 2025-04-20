namespace RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;
using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

public class GetSaleByIdQueryHandler(ISaleRepository saleRepository) : IRequestHandler<GetSaleByIdQuery, Sale>
{
  public async Task<Sale> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
  {
    return await saleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Venda com ID {request.Id} não foi encontrada.");
  }
}
