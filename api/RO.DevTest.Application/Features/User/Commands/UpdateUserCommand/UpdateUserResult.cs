namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

public record UpdateUserResult(
  string Id,
  string Name,
  string Email
)
{
  public UpdateUserResult(Domain.Entities.User user) : this(
    Id: user.Id,
    Name: user.Name!,
    Email: user.Email!
  ) { }
}
