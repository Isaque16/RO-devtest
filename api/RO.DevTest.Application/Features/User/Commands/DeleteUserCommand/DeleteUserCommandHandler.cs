namespace RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

using MediatR;
using Contracts.Persistance.Repositories;

/// <summary>
/// Handles the deletion of a user entity in the system, based on the provided command.
/// This handler leverages the <see cref="IUserRepository"/> for database operations and
/// implements <see cref="IRequestHandler{TRequest, TResponse}"/> to process the <see cref="DeleteUserCommand"/>.
/// </summary>
public class DeleteUserCommandHandler(IUserRepository userRepo) 
  : IRequestHandler<DeleteUserCommand, bool>
{
  /// <summary>
  /// Processes the deletion of a user based on the provided <see cref="DeleteUserCommand"/>.
  /// </summary>
  /// <param name="request">The command containing the user ID to be deleted.</param>
  /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
  /// <returns>A boolean value indicating whether the user was successfully deleted.</returns>
  public async Task<bool> Handle(
    DeleteUserCommand request, CancellationToken cancellationToken)
  {
    if (request.Id == string.Empty) return false;
    
    var user = await userRepo.GetByIdAsync(request.Id, cancellationToken);
    if (user == null) return false;

    return await userRepo.DeleteAsync(user);
  }
}
