namespace RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;
using MediatR;

public class DeleteSaleCommand : IRequest<bool>
{
  public Guid Id { get; set; } = Guid.Empty;
}
