namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;
using MediatR;
using RO.DevTest.Domain.Entities;

public class DeleteProductCommand(Guid id) : IRequest<bool>
{
  public Guid Id { get; set; } = id;
}
