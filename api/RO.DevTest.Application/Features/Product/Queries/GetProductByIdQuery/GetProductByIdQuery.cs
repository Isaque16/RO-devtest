namespace RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;

using MediatR;

public class GetProductByIdQuery(Guid id) : IRequest<Domain.Entities.Product>
{
  public Guid Id { get; set; } = id;
}
