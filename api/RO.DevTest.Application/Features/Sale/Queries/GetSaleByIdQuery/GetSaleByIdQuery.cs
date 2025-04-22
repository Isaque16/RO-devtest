namespace RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;

using MediatR;

public class GetSaleByIdQuery
  : IRequest<Domain.Entities.Sale>
{
  public Guid Id { get; set; } = Guid.Empty;
}
