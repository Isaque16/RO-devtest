namespace RO.DevTest.Domain.Entities;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

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
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;

    public User() : base() { }
}
