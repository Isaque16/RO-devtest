namespace RO.DevTest.Domain.Entities;

using Abstract;

/// <summary>
/// Represents a <see cref="Product"/> in the API.
/// </summary>
public class Product : BaseEntity 
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
  /// <remarks>Used for displaying the product image</remarks>
  public string ImageUrl { get; set; } = string.Empty;
}
