namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

using MediatR;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Domain.Entities;

public class GetAllSalesQuery(PaginationQuery pagination) : IRequest<PaginatedResult<Sale>>
{
    public PaginationQuery Pagination { get; set; } = pagination;
}
