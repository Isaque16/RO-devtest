namespace RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;

using MediatR;

public class GetProductByIdQuery(string id) : IRequest<Domain.Entities.Product>
{
  public string Id { get; set; } = id;
}
