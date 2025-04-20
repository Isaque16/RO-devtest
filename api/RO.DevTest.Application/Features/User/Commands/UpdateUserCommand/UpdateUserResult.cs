namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

public record UpdateUserResult(
  string Id,
  string Name,
  string UserName,
  string Email
)
{
  public UpdateUserResult(Domain.Entities.User user) : this(
    Id: user.Id,
    Name: user.Name!,
    UserName: user.UserName!,
    Email: user.Email!
  ) { }
}
