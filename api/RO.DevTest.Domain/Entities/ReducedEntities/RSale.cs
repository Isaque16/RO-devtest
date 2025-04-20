namespace RO.DevTest.Domain.Entities.ReducedEntities;

/// <summary>
/// Represents a reduced version of the Sale entity, typically used for lightweight data transfer
/// scenarios where only a subset of Sale information is needed.
/// </summary>
public class RSale
{
    public Guid Id { get; set; } = Guid.Empty;
    
    public List<RProduct> Products { get; set; } = [];

    public int Quantity { get; set; } = 0;

    public decimal TotalPrice { get; set; } = 0.0m;

    public string CustomerId { get; set; } = string.Empty;
}
