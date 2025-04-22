using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;
using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Commands;

using FluentValidation;
using Domain.Entities;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly UpdateUserCommandHandler _handler;
    
    public UpdateUserCommandHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new UpdateUserCommandHandler(_userRepoMock.Object);
    }
    
    private static UpdateUserCommand GenerateValidCommand()
    {
        return new Faker<UpdateUserCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid().ToString())
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.UserName, f => f.Internet.UserName())
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Role, f => f.PickRandom(UserRoles.Admin, UserRoles.Customer))
            .Generate();
    }
    
    [Fact]
    public async Task Handle_UpdatesUserSuccessfully_WhenDataIsValid()
    {
        var command = GenerateValidCommand();

        var existingUser = new User
        {
            Id = command.Id,
            Name = "Old Name",
            UserName = "OldUserName",
            Email = "old@example.com",
            PhoneNumber = "0987654321",
            Role = UserRoles.Customer
        };

        var updatedUser = command.AssignTo(existingUser);

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _userRepoMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(updatedUser);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(updatedUser.Id, result.Id);
        Assert.Equal(updatedUser.Name, result.Name);
        Assert.Equal(updatedUser.UserName, result.UserName);
        Assert.Equal(updatedUser.Email, result.Email);
        Assert.Equal(updatedUser.PhoneNumber, result.PhoneNumber);
        Assert.Equal(updatedUser.Role, result.Role);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenCommandIsInvalid()
    {
        var command = new UpdateUserCommand(); // Invalid command with missing data

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
    {
        var command = GenerateValidCommand();

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*User with id {command.Id} not found*");
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenUserUpdateFails()
    {
        var command = GenerateValidCommand();

        var existingUser = new User
        {
            Id = command.Id,
            Name = "Old Name",
            UserName = "OldUserName",
            Email = "old@example.com",
            PhoneNumber = "0987654321",
            Role = UserRoles.Customer
        };

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _userRepoMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync((User)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"*Failed to update user with id {command.Id}*");
    }
}
