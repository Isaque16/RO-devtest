using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

using MediatR;
using RO.DevTest.Domain.Entities;

public class UpdateUserCommand : IRequest<UpdateUserResult>
{
  public string Id { get; set; } = string.Empty;
  
  public string Name { get; set; } = string.Empty;
  
  public string UserName { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public string PhoneNumber { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  
  public UserRoles Role { get; set; }
  

  public User AssignTo(User user)
  {
    user.Name = Name;
    user.UserName = UserName;
    user.Password = Password;
    user.Email = Email;
    user.PhoneNumber = PhoneNumber;
    user.Role = Role;
    return user;
  }
}
