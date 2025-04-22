using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Domain.Entities.ReducedEntities;

/// <summary>
/// Represents a reduced version of a user entity.
/// </summary>
public class RUser
{
    public string Id { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;   
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRoles Role { get; set; } = UserRoles.Customer;
}
