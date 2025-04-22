
using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Domain.Entities;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a <see cref="IdentityUser"/> int the API
/// </summary>
public class User : IdentityUser 
{
    /// <summary>
    /// Name of the user
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user
    /// </summary>
    /// <remarks>Used for authentication</remarks>
    [JsonIgnore]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    
    public UserRoles Role { get; set; } = UserRoles.Customer;

    /// <summary>
    /// A collection of sales associated with the user.
    /// </summary>
    public ICollection<Sale>? Sales { get; set; } = new List<Sale>();
}
