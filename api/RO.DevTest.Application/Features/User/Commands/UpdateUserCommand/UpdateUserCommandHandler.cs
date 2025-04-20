using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

/// <summary>
/// Handler for updating a user.
/// </summary>
/// <param name="userRepository"></param>
public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
  /// <summary>
  /// Handles the update user command.
  /// Validates the request, retrieves the user by ID, updates it, and returns the updated user.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns>
  ///  Returns the updated user.
  /// </returns>
  /// <exception cref="BadRequestException"></exception>
  /// <exception cref="KeyNotFoundException"></exception>
  /// <exception cref="Exception"></exception>
  public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
  {
    UpdateUserCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      throw new BadRequestException(validationResult);
    
    var user = await userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"User with id {request.Id} not found.");
    user = request.AssignTo(user);

    var updatedUser = await userRepository.UpdateAsync(user) ?? throw new Exception($"Failed to update user with id {request.Id}.");
    return new UpdateUserResult
    {
      Id = updatedUser.Id,
      Name = updatedUser.Name,
      Email = updatedUser.Email!,
    };
  }
}
