using Bogus;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Commands;

using Moq;
using Domain.Enums;
using FluentAssertions;
using FluentValidation;
using RO.DevTest.Domain.Exception;
using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserCommandHandlerTests {
    private readonly Mock<IIdentityAbstractor> _identityAbstractorMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests() 
    { 
        _identityAbstractorMock = new Mock<IIdentityAbstractor>();
        _handler = new CreateUserCommandHandler(_identityAbstractorMock.Object);
    }

    private static CreateUserCommand GenerateValidCommand()
    {
        return new Faker<CreateUserCommand>()
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.UserName, f => f.Internet.UserName())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f => f.Internet.Password())
            .RuleFor(c => c.Role, f => f.PickRandom(UserRoles.Admin, UserRoles.Customer))
            .Generate();
    }

    [Fact]
    public async Task Handle_CreatesUserAndAssignsRole_WhenDataIsValid()
    {
        // Arrange
        var command = GenerateValidCommand();

        var newUser = command.AssignTo();
        var creationResult = IdentityResult.Success;
        var roleResult = IdentityResult.Success;

        // Act
        _identityAbstractorMock
            .Setup(i => i.CreateUserAsync(newUser, command.Password))
            .ReturnsAsync(creationResult);
        _identityAbstractorMock
            .Setup(i => i.AddToRoleAsync(newUser, command.Role))
            .ReturnsAsync(roleResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(command.UserName, result.UserName);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand(); // Invalid command with missing data

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
    }

    [Fact]
    public async Task Handle_ThrowsBadRequestException_WhenUserCreationFails()
    {
        // Arrange
        var command = GenerateValidCommand();
        
        var newUser = command.AssignTo();
        var creationResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });

        _identityAbstractorMock
            .Setup(i => i.CreateUserAsync(newUser, command.Password))
            .ReturnsAsync(creationResult);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*User creation failed*");
    }

    [Fact]
    public async Task Handle_ThrowsBadRequestException_WhenRoleAssignmentFails()
    {
        // Arrange
        var command = GenerateValidCommand();

        var newUser = command.AssignTo();
        var creationResult = IdentityResult.Success;
        var roleResult = IdentityResult.Failed(new IdentityError { Description = "Role assignment failed" });

        _identityAbstractorMock
            .Setup(i => i.CreateUserAsync(newUser, command.Password))
            .ReturnsAsync(creationResult);
        _identityAbstractorMock
            .Setup(i => i.AddToRoleAsync(newUser, command.Role))
            .ReturnsAsync(roleResult);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*Role assignment failed*");
    }
}
