namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Queries;

using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;
using Domain.Entities;

public class GetSaleByIdQueryHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly GetSaleByIdQueryHandler _handler;
    
    public GetSaleByIdQueryHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new GetSaleByIdQueryHandler(_saleRepoMock.Object);
    }
    
    private static GetSaleByIdQuery GenerateValidQuery()
    {
        return new Faker<GetSaleByIdQuery>()
            .CustomInstantiator(f => new GetSaleByIdQuery(f.Random.Guid()));
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSale_WhenSaleExists()
    {
        // Arrange
        var query = GenerateValidQuery();

        var sale = new Faker<Sale>()
            .RuleFor(s => s.Id, query.Id)
            .Generate();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sale);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equivalent(sale, result);
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
    {
        // Arrange
        var query = GenerateValidQuery();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sale)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Venda com ID {query.Id} não foi encontrada.");
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
