namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

using RO.DevTest.Domain.Entities;
using MediatR;

/// <summary>
/// Command to create a new <see cref="Product"/>
/// </summary>
public class CreateProductCommand : IRequest<Product>
{
    /// <summary>
    /// Name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product
    /// </summary>
    public decimal Price { get; set; } = 0.0m;

    /// <summary>
    /// Quantity of the product in stock
    /// </summary>
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// Image URL of the product
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Maps the command to a <see cref="Product"/>
    /// </summary>
    public Product AssignTo()
    {
        return new Product
        {
            Name = Name,
            Description = Description,
            Price = Price,
            Quantity = Quantity,
            ImageUrl = ImageUrl
        };
    }
}
