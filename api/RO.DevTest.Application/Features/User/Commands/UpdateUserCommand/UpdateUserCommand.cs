namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

using MediatR;
using RO.DevTest.Domain.Entities;

public class UpdateUserCommand : IRequest<UpdateUserResult>
{
  public Guid Id { get; set; }
  
  public string Name { get; set; } = string.Empty;
  
  public string UserName { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public User AssignTo(User user)
  {
    user.Name = Name;
    user.UserName = UserName;
    user.Password = Password;
    user.Email = Email;
    return user;
  }
}
