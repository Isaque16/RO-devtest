namespace RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

using MediatR;

public class DeleteUserCommand(string id) : IRequest<bool>
{
  public string Id { get; set; } = id;
}
