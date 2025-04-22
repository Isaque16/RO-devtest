namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

using MediatR;
using Domain.Entities;

public class GetAllSalesQuery 
    : IRequest<PaginatedResult<Sale>>
{
    public PaginationQuery Pagination { get; set; } = new();
}
