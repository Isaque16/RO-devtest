using System.ComponentModel.DataAnnotations;

namespace RO.DevTest.Domain.Entities.ReducedEntities;

/// <summary>
/// Represents a reduced version of the Product entity, typically used for lightweight data transfer
/// scenarios where only a subset of product information is needed.
/// </summary>
public class RProduct
{
    public RProduct()
    {
    }

    public RProduct(Guid id, string name, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public Guid Id { get; set; } = Guid.Empty;
    
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [MaxLength(200)]
    public string ImageUrl { get; set; } = string.Empty;
}
