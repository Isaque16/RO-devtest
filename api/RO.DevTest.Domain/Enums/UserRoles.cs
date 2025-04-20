namespace RO.DevTest.Domain.Enums;

using System.ComponentModel;

/// <summary>
/// This enum represents the different user roles in the system.
/// </summary>
public enum UserRoles {
    [Description("Admin")]
    Admin = 0,
    [Description("Customer")]
    Customer = 1,
}
