namespace RO.DevTest.Domain.Entities.ReducedEntities;

/// <summary>
/// Represents a reduced version of the Product entity, typically used for lightweight data transfer
/// scenarios where only a subset of product information is needed.
/// </summary>
public class RProduct
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}
