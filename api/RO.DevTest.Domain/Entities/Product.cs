namespace RO.DevTest.Domain.Entities;

using Abstract;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a <see cref="Product"/> in the API.
/// </summary>
public class Product : BaseEntity
{
  /// <summary>
  /// Name of the product
  /// </summary>
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Description of the product
  /// </summary>
  [MaxLength(500)]
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Price of the product
  /// </summary>
  public decimal Price { get; set; }

  /// <summary>
  /// Quantity of the product in stock
  /// </summary>
  public int Quantity { get; set; }

  /// <summary>
  /// Image URL of the product
  /// </summary>
  /// <remarks>Used for displaying the product image</remarks>
  [MaxLength(200)]
  public string ImageUrl { get; set; } = string.Empty;
}
