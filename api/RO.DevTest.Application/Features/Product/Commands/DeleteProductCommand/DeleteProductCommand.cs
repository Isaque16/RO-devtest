namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;
using MediatR;

public class DeleteProductCommand : IRequest<bool>
{
  public Guid Id { get; set; } = Guid.Empty;
}
