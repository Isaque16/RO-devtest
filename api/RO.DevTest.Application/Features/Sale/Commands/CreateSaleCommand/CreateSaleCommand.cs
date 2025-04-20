namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using MediatR;
using Domain.Entities;

public class CreateSaleCommand : IRequest<Sale>
{
  public List<Product> Products { get; set; } = [];

  public int Quantity { get; set; } = 0;

  public decimal TotalPrice { get; set; } = 0.0m;

  public string CustomerId { get; set; } = string.Empty;

  public User Customer { get; set; } = new();

  public Sale AssignTo()
  {
    return new Sale
    {
      Products = Products,
      Quantity = Quantity,
      TotalPrice = TotalPrice,
      CustomerId = CustomerId,
      Customer = Customer
    };
  }
}
