namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

using MediatR;
using Domain.Entities;
using Domain.Enums;

public class CreateUserCommand : IRequest<CreateUserResult> 
{
    public string Name { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public UserRoles Role { get; set; } = UserRoles.Customer;

    public User AssignTo() 
    {
        return new User 
        {
            Name = Name,
            UserName = UserName,
            Password = Password,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Role = Role
        };
    }
}
