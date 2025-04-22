namespace RO.DevTest.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using ReducedEntities;
using Abstract;

/// <summary>
/// Represents a <see cref="Sale"/> in the API.
/// </summary>
public class Sale : BaseEntity 
{
  /// <summary>
  /// List of products sold in the sale
  /// </summary>
  public List<RProduct> Products { get; set; } = [];

  /// <summary>
  /// The total quantity of products sold in the sale
  /// </summary>
  public int Quantity => Products.Sum(item => item.Quantity);

  /// <summary>
  /// The total price of the sale
  /// </summary>
  public decimal TotalPrice => Products.Sum(item => item.Price * item.Quantity);

  /// <summary>
  /// The ID of the customer who made the sale
  /// </summary>
  [MaxLength(100)]
  public string CustomerId { get; set; } = string.Empty;
  
  /// <summary>
  /// The customer who made the sale
  /// </summary>
  public RUser Customer { get; set; } = new();
}
