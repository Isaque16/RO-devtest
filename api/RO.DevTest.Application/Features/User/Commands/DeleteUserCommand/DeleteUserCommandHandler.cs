namespace RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
{
  public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  { 
    if (string.IsNullOrEmpty(request.Id)) return false;
    
    var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
    if (user == null) return false;

    return await userRepository.DeleteAsync(user);
  }
}
