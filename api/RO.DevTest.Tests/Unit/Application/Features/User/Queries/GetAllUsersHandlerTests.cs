namespace RO.DevTest.Tests.Unit.Application.Features.User.Queries;

using Bogus;
using FluentAssertions;
using Moq;
using Domain.Entities;
using RO.DevTest.Application.Features;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;

public class GetAllUsersHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly GetAllUsersQueryHandler _handler;

    public GetAllUsersHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new GetAllUsersQueryHandler(_userRepoMock.Object);
    }

    private static GetAllUsersQuery GenerateValidQuery()
    {
        return new Faker<GetAllUsersQuery>()
            .RuleFor(q => q.Pagination, f => new PaginationQuery
            {
                PageNumber = f.Random.Int(1, 10),
                PageSize = f.Random.Int(1, 100)
            })
            .Generate();
    }

    private static List<User> GenerateUsers(int count)
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.String())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .Generate(count);
    }
    
    [Fact]
    public async Task Handle_ReturnsPaginatedResult_WhenUsersExist()
    {
        // Arrange
        var query = GenerateValidQuery();
        var users = GenerateUsers(10);
        var paginatedUsers = new PaginatedResult<User>(users, 10, 1, 10);

        _userRepoMock
            .Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedUsers);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(10, result.Content.Count);
        Assert.All(result.Content, item => Assert.IsType<GetUserResult>(item));
        Assert.Equal(10, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyPaginatedResult_WhenNoUsersExist()
    {
        // Arrange
        var query = GenerateValidQuery();
        var paginatedUsers = new PaginatedResult<User>([], 0, 1, 10);

        _userRepoMock
            .Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedUsers);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.Content);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
    {
        // Arrange
        var query = GenerateValidQuery();

        _userRepoMock
            .Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
