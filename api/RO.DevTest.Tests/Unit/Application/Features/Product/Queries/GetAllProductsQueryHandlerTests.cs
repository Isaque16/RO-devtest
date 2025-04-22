using Bogus;
using Moq;
using RO.DevTest.Application.Features;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Queries;

using Domain.Entities;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _handler = new GetAllProductsQueryHandler(_productRepoMock.Object);
    }
    
    private static GetAllProductsQuery GenerateValidQuery()
    {
        return new Faker<GetAllProductsQuery>()
            .RuleFor(p => p.PaginationQuery, f => new PaginationQuery
            {
                PageNumber = f.Random.Int(1, 10),
                PageSize = f.Random.Int(1, 100)
            });
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResult_WhenProductsExist()
    {
        // Arrange
        var query = GenerateValidQuery();
        var products = new PaginatedResult<Product>(
            [new Product { Id = Guid.NewGuid(), Name = "Product1" }],
            1, 1, 10);
        _productRepoMock
            .Setup(repo => repo.GetAllPagedAsync(query.PaginationQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Content);
        Assert.Equal("Product1", result.Content.First().Name);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyPaginatedResult_WhenNoProductsExist()
    {
        // Arrange
        var query = GenerateValidQuery();
        var products = new PaginatedResult<Product>([], 0, 1, 10);
        _productRepoMock
            .Setup(repo => repo.GetAllPagedAsync(query.PaginationQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Content);
        Assert.Equal(0, result.TotalCount);
    }
}
