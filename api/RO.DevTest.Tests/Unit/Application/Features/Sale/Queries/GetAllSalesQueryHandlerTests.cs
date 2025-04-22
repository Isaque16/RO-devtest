using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features;
using RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Queries;

using Domain.Entities;

public class GetAllSalesQueryHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly GetAllSalesQueryHandler _handler;
    
    public GetAllSalesQueryHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new GetAllSalesQueryHandler(_saleRepoMock.Object);
    }
    
    private static GetAllSalesQuery GenerateValidQuery()
    {
        return new Faker<GetAllSalesQuery>()
            .RuleFor(q => q.Pagination, f => new PaginationQuery
            {
                PageNumber = f.Random.Int(1, 10),
                PageSize = f.Random.Int(1, 100)
            });
    }
    
    [Fact]
    public async Task Handle_ShouldReturnPaginatedResult_WhenSalesExist()
    {
        var query = GenerateValidQuery();

        var sales = new Faker<Sale>()
            .Generate(10);

        var paginatedResult = new PaginatedResult<Sale>(sales, sales.Count, query.Pagination.PageNumber, query.Pagination.PageSize);

        _saleRepoMock.Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(paginatedResult);
        _saleRepoMock.Verify(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyPaginatedResult_WhenNoSalesExist()
    {
        var query = GenerateValidQuery();

        var paginatedResult = new PaginatedResult<Sale>(new List<Sale>(), 0, query.Pagination.PageNumber, query.Pagination.PageSize);

        _saleRepoMock.Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(paginatedResult);
        _saleRepoMock.Verify(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
    {
        var query = GenerateValidQuery();

        _saleRepoMock.Setup(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Repository error"));

        var act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("Repository error");
        _saleRepoMock.Verify(repo => repo.GetAllPagedAsync(query.Pagination, It.IsAny<CancellationToken>()), Times.Once);
    }
}
