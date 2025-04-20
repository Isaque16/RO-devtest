namespace RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;

using MediatR;

public class GetSaleByIdQuery(string id) 
  : IRequest<Domain.Entities.Sale>
{
  public string Id { get; set; } = id;
}
