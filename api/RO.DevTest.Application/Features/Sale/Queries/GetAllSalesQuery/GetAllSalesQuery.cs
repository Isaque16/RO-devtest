namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

using MediatR;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Domain.Entities;

public class GetAllSalesQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PaginatedResult<Sale>>
{
  public int PageNumber { get; set; } = pageNumber;

  public int PageSize { get; set; } = pageSize;
}
