using RO.DevTest.Domain.Abstract;

namespace RO.DevTest.Domain.Entities;

/// <summary>
/// Represents a <see cref="Sale"/> in the API.
/// </summary>
public class Sale : BaseEntity 
{
  /// <summary>
  /// List of products sold in the sale
  /// </summary>
  public List<Product> Products { get; set; } = [];

  /// <summary>
  /// The total quantity of products sold in the sale
  /// </summary>
  public int Quantity { get; set; } = 0;

  /// <summary>
  /// The total price of the sale
  /// </summary>
  public decimal TotalPrice { get; set; } = 0.0m;

  /// <summary>
  /// The ID of the customer who made the sale
  /// </summary>
  public string CustomerId { get; set; } = string.Empty;
  
  /// <summary>
  /// The customer who made the sale
  /// </summary>
  public User Customer { get; set; } = new();
}