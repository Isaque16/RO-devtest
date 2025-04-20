namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

using MediatR;
using Domain.Entities;

/// <summary>
/// Command to update a product.
/// </summary>
public class UpdateProductCommand : IRequest<Product>
{
  public Guid Id { get; set; } = Guid.Empty;
  
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  public decimal Price { get; set; }
  
  public int Quantity { get; set; }

  public string ImageUrl { get; set; } = string.Empty;

  /// <summary>
  /// Assigns the properties of this command to the given product entity.
  /// </summary>
  public Product AssignTo(Product product)
  {
    product.Name = Name;
    product.Description = Description;
    product.Price = Price;
    product.Quantity = Quantity;
    product.ImageUrl = ImageUrl;
    
    return product;
  }
}
