namespace RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;
using MediatR;

public class DeleteSaleCommand(Guid id) : IRequest<bool>
{
  public Guid Id { get; set; } = id;
}
