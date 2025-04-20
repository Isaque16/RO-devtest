using FluentValidation;

namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

using Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;
using MediatR;

/// <summary>
/// Handler for updating a user.
/// </summary>
/// <param name="userRepo"></param>
public class UpdateUserCommandHandler(IUserRepository userRepo) : IRequestHandler<UpdateUserCommand, UpdateUserResult>
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
      throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);
    
    var user = await userRepo.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"User with id {request.Id} not found.");
    user = request.AssignTo(user);

    var updatedUser = await userRepo.UpdateAsync(user) ?? throw new Exception($"Failed to update user with id {request.Id}.");
    return new UpdateUserResult
    (
      Id: updatedUser.Id,
      Name: updatedUser.Name,
      UserName: updatedUser.UserName!,
      Email: updatedUser.Email!
    );
  }
}
