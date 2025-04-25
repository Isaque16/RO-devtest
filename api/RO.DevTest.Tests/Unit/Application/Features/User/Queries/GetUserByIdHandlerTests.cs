namespace RO.DevTest.Tests.Unit.Application.Features.User.Queries;

using Bogus;
using FluentAssertions;
using Moq;
using Domain.Entities;
using RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

public class GetUserByIdHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepoMock.Object);
    }
    
    private static GetUserByIdQuery GenerateValidQuery()
    {
        return new Faker<GetUserByIdQuery>()
            .CustomInstantiator(f => new GetUserByIdQuery(f.Random.String()))
            .Generate();
    }

    private static User GenerateUser()
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.String())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .Generate();
    }

    [Fact]
    public async Task Handle_ReturnsUserResult_WhenUserExists()
    {
        // Arrange
        var query = GenerateValidQuery();
        var user = GenerateUser();
        
        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Equal(user.PhoneNumber, result.PhoneNumber);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var query = GenerateValidQuery();
        
        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Usuário com ID {query.Id} não foi encontrado.");
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var query = GenerateValidQuery();

        _userRepoMock
            .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
