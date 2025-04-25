namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;
using MediatR;

public class DeleteProductCommand(Guid id) : IRequest<bool>
{
  public Guid Id { get; set; } = id;
}
