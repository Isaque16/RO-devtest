using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Commands;

using Domain.Entities;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly DeleteUserCommandHandler _handler;
    
    public DeleteUserCommandHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new DeleteUserCommandHandler(_userRepoMock.Object);
    }

    private static DeleteUserCommand GenerateValidCommand()
    {
        return new Faker<DeleteUserCommand>()
            .RuleFor(u => u.Id, f => f.Random.Guid().ToString());
    }
    
    [Fact]
    public async Task Handle_ReturnsTrue_WhenUserIsSuccessfullyDeleted()
    {
        // Arrange
        var command = GenerateValidCommand();
        var user = new User { Id = command.Id };

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepoMock
            .Setup(repo => repo.DeleteAsync(user))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = string.Empty };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid().ToString() };

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_ReturnsFalse_WhenDeleteOperationFails()
    {
        // Arrange
        var command = new DeleteUserCommand { Id = Guid.NewGuid().ToString() };
        var user = new User { Id = command.Id };

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepoMock
            .Setup(repo => repo.DeleteAsync(user))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
