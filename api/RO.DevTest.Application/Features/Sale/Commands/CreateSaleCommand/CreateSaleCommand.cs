using RO.DevTest.Domain.Entities.ReducedEntities;

namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using MediatR;
using Domain.Entities;

public class CreateSaleCommand : IRequest<Sale>
{
  public List<RProduct> Products { get; set; } = [];

  public int Quantity { get; set; } = 0;

  public decimal TotalPrice { get; set; } = 0.0m;

  public string CustomerId { get; set; } = string.Empty;

  public Sale AssignTo()
  {
    return new Sale
    {
      Products = Products.ConvertAll(product => product.ToProduct()),
      Quantity = Quantity,
      TotalPrice = TotalPrice,
      CustomerId = CustomerId,
    };
  }
}
