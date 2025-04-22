namespace RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;

using MediatR;

public class GetProductByIdQuery : IRequest<Domain.Entities.Product>
{
  public Guid Id { get; set; } = Guid.Empty;
}
