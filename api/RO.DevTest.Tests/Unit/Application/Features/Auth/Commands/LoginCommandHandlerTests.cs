using Bogus;
using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Tests.Unit.Application.Features.Auth.Commands;

using Moq;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommandHandlerTests
{
    private readonly Mock<IIdentityAbstractor> _identityAbstractorMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginCommandHandler _handler;
    
    public LoginCommandHandlerTests()
    {
        _identityAbstractorMock = new Mock<IIdentityAbstractor>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new LoginCommandHandler(_identityAbstractorMock.Object, _tokenServiceMock.Object);
    }

    private static LoginCommand GenerateValidCommand()
    {
        return new Faker<LoginCommand>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password());
    }

    private static User GenerateExistingUser(string username, string password)
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid().ToString())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.UserName, () => username)
            .RuleFor(u => u.Password, () => password)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Role, f => f.PickRandom(UserRoles.Admin, UserRoles.Customer));
    }
    
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var command = GenerateValidCommand();
        var existingUser = GenerateExistingUser(command.Username, command.Password);
        const string accessToken = "validAccessToken";
        const string refreshToken = "validRefreshToken";

        
        _identityAbstractorMock.Setup(x => x.FindUserByUserNameAsync(existingUser.UserName!))
            .ReturnsAsync(existingUser);
        _identityAbstractorMock.Setup(x => x.PasswordSignInAsync(existingUser, existingUser.Password))
            .ReturnsAsync(SignInResult.Success);
        _identityAbstractorMock.Setup(x => x.GetUserRolesAsync(existingUser))
            .ReturnsAsync(["Admin"]);
        _tokenServiceMock.Setup(x => x.GenerateAccessToken(existingUser, new List<string> { existingUser.Role.ToString() }))
            .Returns(accessToken);
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(accessToken, result.AccessToken);
        Assert.Equal(refreshToken, result.RefreshToken);
        Assert.Equal(new List<string> { existingUser.Role.ToString() }, result.Roles);
        Assert.True(result.ExpirationDate > DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_InvalidUsername_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        const string username = "invalidUser";

        _identityAbstractorMock.Setup(x => x.FindUserByUserNameAsync(username))
            .ReturnsAsync((User)null);

        var command = GenerateValidCommand();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var command = GenerateValidCommand();
        var existingUser = GenerateExistingUser(command.Username, command.Password);

        _identityAbstractorMock.Setup(x => x.FindUserByUserNameAsync(existingUser.UserName!))
            .ReturnsAsync(existingUser);
        _identityAbstractorMock.Setup(x => x.PasswordSignInAsync(existingUser, existingUser.Password))
            .ReturnsAsync(SignInResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_EmptyRoles_ReturnsLoginResponseWithNoRoles()
    {
        // Arrange
        var command = GenerateValidCommand();
        var existingUser = GenerateExistingUser(command.Username, command.Password);
        var roles = new List<string>();
        const string accessToken = "validAccessToken";
        const string refreshToken = "validRefreshToken";

        _identityAbstractorMock.Setup(x => x.FindUserByUserNameAsync(command.Username))
            .ReturnsAsync(existingUser);
        _identityAbstractorMock.Setup(x => x.PasswordSignInAsync(existingUser, existingUser.Password))
            .ReturnsAsync(SignInResult.Success);
        _identityAbstractorMock.Setup(x => x.GetUserRolesAsync(existingUser))
            .ReturnsAsync(roles);
        _tokenServiceMock.Setup(x => x.GenerateAccessToken(existingUser, roles))
            .Returns(accessToken);
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(accessToken, result.AccessToken);
        Assert.Equal(refreshToken, result.RefreshToken);
        Assert.Empty(result.Roles!);
        Assert.True(result.ExpirationDate > DateTime.UtcNow);
    }
}