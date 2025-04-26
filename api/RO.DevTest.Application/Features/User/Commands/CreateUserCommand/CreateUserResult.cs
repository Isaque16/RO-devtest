using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

public record CreateUserResult(
    string Id,
    string Name,
    string UserName,
    string Email,
    string PhoneNumber,
    UserRoles Role)
{
    public CreateUserResult(Domain.Entities.User user) 
        : this(
            Id: user.Id,
            Name: user.Name!,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber!,
            Role: UserRoles.Customer
        ) { }
}