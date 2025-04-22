namespace RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

using MediatR;

public class DeleteUserCommand : IRequest<bool>
{
  public string Id { get; set; } = string.Empty;
}
