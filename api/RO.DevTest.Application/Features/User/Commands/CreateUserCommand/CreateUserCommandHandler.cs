namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

using RO.DevTest.Domain.Exception;
using Contracts.Infrastructure;
using FluentValidation;
using MediatR;

/// <summary>
/// Command handler for the creation of <see cref="Domain.Entities.User"/>
/// </summary>
public class CreateUserCommandHandler(IIdentityAbstractor identityAbstractor) 
    : IRequestHandler<CreateUserCommand, CreateUserResult> 
{
    /// <summary>
    /// Handles the execution of the CreateUserCommand, which processes user creation and role assignment.
    /// </summary>
    /// <param name="request">The command containing user details and credentials required for user creation.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the details of the created user.</returns>
    public async Task<CreateUserResult> Handle(
        CreateUserCommand request, CancellationToken cancellationToken) 
    {
        CreateUserCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) 
            throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);

        var newUser = request.AssignTo();
        var userCreationResult = await identityAbstractor.CreateUserAsync(newUser, request.Password);
        
        if (!userCreationResult.Succeeded) 
            throw new BadRequestException(userCreationResult);

        var userRoleResult = await identityAbstractor.AddToRoleAsync(newUser, request.Role);
        
        if (!userRoleResult.Succeeded) 
            throw new BadRequestException(userRoleResult);

        return new CreateUserResult(newUser);
    }
}
