using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Queries;

using Domain.Entities;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _handler = new GetProductByIdQueryHandler(_productRepoMock.Object);
    }
    
    private static GetProductByIdQuery GenerateValidQuery()
    {
        return new Faker<GetProductByIdQuery>()
            .CustomInstantiator(f => new GetProductByIdQuery(f.Random.Guid()));
    }

    [Fact]
    public async Task Handle_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var query = GenerateValidQuery();
        var product = new Product
        {
            Id = query.Id,
            Name = "Product1"
        };
        
        _productRepoMock
            .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("Product1", result.Name);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var query = GenerateValidQuery();

        _productRepoMock
            .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
