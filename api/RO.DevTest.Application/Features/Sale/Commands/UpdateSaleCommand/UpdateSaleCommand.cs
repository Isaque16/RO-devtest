namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using Domain.Entities.ReducedEntities;
using Domain.Entities;
using MediatR;

public class UpdateSaleCommand : IRequest<Sale>
{
  public Guid Id { get; set; } = Guid.Empty;
  
  public List<RProduct> Products { get; set; } = [];
  
  public string CustomerId { get; set; } = string.Empty;

  public Sale AssignTo(Sale sale)
  {
    sale.Products = Products;
    sale.CustomerId = CustomerId;
    return sale;
  }
}
