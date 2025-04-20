namespace RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;
using MediatR;

public class DeleteSaleCommand(string id) : IRequest<bool>
{
  public string Id { get; set; } = id;
}
