namespace RO.DevTest.Domain.Entities.ReducedEntities;

/// <summary>
/// Provides extension methods for mapping between reduced entity types and full entity types.
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Maps an instance of <see cref="RProduct"/> to an instance of <see cref="Product"/>.
    /// </summary>
    /// <param name="rProduct">
    /// The reduced product entity to be mapped.
    /// </param>
    /// <returns>
    /// A <see cref="Product"/> instance containing the data from the provided <see cref="RProduct"/>.
    /// </returns>
    public static Product ToProduct(this RProduct rProduct)
    {
        return new Product
        {
            Id = rProduct.Id,
            Name = rProduct.Name,
            Description = rProduct.Description,
            Price = rProduct.Price,
            Quantity = rProduct.Quantity,
            ImageUrl = rProduct.ImageUrl
        };
    }

    /// <summary>
    /// Maps an instance of <see cref="RUser"/> to an instance of <see cref="User"/>.
    /// </summary>
    /// <param name="rUser">
    /// The reduced user entity to be mapped.
    /// </param>
    /// <returns>
    /// A <see cref="User"/> instance containing the data from the provided <see cref="RUser"/>.
    /// </returns>
    public static User ToUser(this RUser rUser)
    {
        return new User
        {
            Id = rUser.Id,
            Name = rUser.Name,
            UserName = rUser.UserName,
            PhoneNumber = rUser.PhoneNumber,
            Email = rUser.Email
        };
    }

    /// <summary>
    /// Maps an instance of <see cref="RSale"/> to an instance of <see cref="Sale"/>.
    /// </summary>
    /// <param name="rSale">
    /// The reduced sale entity to be mapped.
    /// </param>
    /// <returns>
    /// A <see cref="Sale"/> instance containing the data from the provided <see cref="RSale"/>.
    /// </returns>
    public static Sale ToSale(this RSale rSale)
    {
        return new Sale
        {
            Id = rSale.Id,
            Products = rSale.Products.ConvertAll(p => p.ToProduct()),
            Quantity = rSale.Quantity,
            TotalPrice = rSale.TotalPrice,
            CustomerId = rSale.CustomerId,
        };
    }
}
