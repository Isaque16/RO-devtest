using RO.DevTest.Domain.Entities.ReducedEntities;

namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using MediatR;
using Domain.Entities;

public class CreateSaleCommand : IRequest<Sale>
{
  public List<RProduct> Products { get; set; } = [];

  public string CustomerId { get; set; } = string.Empty;

  public Sale AssignTo()
  {
    return new Sale
    {
      Products = Products,
      CustomerId = CustomerId,
    };
  }
}
