namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Domain.Entities;

public class GetAllSalesQueryHandler(ISaleRepository saleRepository) : IRequestHandler<GetAllSalesQuery, PaginatedResult<Sale>>
{
  public async Task<PaginatedResult<Sale>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
  {
    // Busca as vendas paginadas no banco de dados
    var (sales, totalCount) = await saleRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

    // Mapeia as vendas para o resultado da query
    var result = sales.Select(sale => new Sale
    {
      Id = sale.Id,
      CustomerId = sale.CustomerId,
      Products = sale.Products,
      Quantity = sale.Quantity,
      TotalPrice = sale.TotalPrice
    }).ToList();

    return new PaginatedResult<Sale>(result, totalCount, request.PageNumber, request.PageSize);
  }
}
