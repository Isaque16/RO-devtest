namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

using MediatR;
using Domain.Entities;
using Domain.Enums;

public class CreateUserCommand : IRequest<CreateUserResult> 
{
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirmation { get; set; } = string.Empty;
    public UserRoles Role { get; set; }

    public User AssignTo() 
    {
        return new User 
        {
            UserName = UserName,
            Email = Email,
            Name = Name,
        };
    }
}
