using Microsoft.AspNetCore.Identity;

namespace RO.DevTest.Domain.Entities;

/// <summary>
/// Represents a <see cref="IdentityUser"/> int the API
/// </summary>
public class User : IdentityUser 
{
    /// <summary>
    /// Name of the user
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user
    /// </summary>
    /// <remarks>Used for authentication</remarks>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// List of sales made by the user
    /// </summary>
    /// <remarks>Used for displaying the sales made by the user</remarks>
    ICollection<Sale> Sales { get; set; } = [];

    public User() : base() { }
}
