namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using MediatR;
using RO.DevTest.Domain.Entities;

public class UpdateSaleCommand : IRequest<Sale>
{
  public string Id { get; set; } = string.Empty;
  public List<Product> Products { get; set; } = [];
  public int Quantity { get; set; }
  public decimal TotalPrice { get; set; }

  public string CustomerId { get; set; } = string.Empty;

  public User Customer { get; set; } = new();

  public Sale AssignToSale(Sale sale)
  {
    sale.Products = Products;
    sale.Quantity = Quantity;
    sale.TotalPrice = TotalPrice;
    sale.CustomerId = CustomerId;
    sale.Customer = Customer;
    return sale;
  }
}
