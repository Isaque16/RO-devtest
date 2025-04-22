using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features;
using RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;
using RO.DevTest.Domain.Entities.ReducedEntities;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Queries;

using Domain.Entities;

public class GetAllSalesByPeriodQueryHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly GetAllSalesByPeriodQueryHandler _handler;
    
    public GetAllSalesByPeriodQueryHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new GetAllSalesByPeriodQueryHandler(_saleRepoMock.Object);
    }
    
    private static GetAllSalesByPeriodQuery GenerateValidQuery()
    {
        return new Faker<GetAllSalesByPeriodQuery>()
            .RuleFor(q => q.DateRange, f => new DateTimeRange(f.Date.Past(), f.Date.Recent()))
            .RuleFor(q => q.Pagination, f => new PaginationQuery
            {
                PageNumber = f.Random.Int(1, 10),
                PageSize = f.Random.Int(1, 50)
            })
            .Generate();
    }

    private static List<Sale> CreateTestSales(int count)
    {
        return new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.CreatedOn, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now))
            .RuleFor(s => s.Products, f => new Faker<RProduct>()
                .RuleFor(p => p.Id, () => f.Random.Guid())
                .RuleFor(p => p.Name, () => f.Commerce.ProductName())
                .RuleFor(p => p.Price, () => f.Random.Decimal(1, 100))
                .RuleFor(p => p.Quantity, () => f.Random.Int(1, 5))
                .Generate(f.Random.Int(1, 3)))
            .Generate(count);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectProductRevenues_WhenSalesHaveMultipleProducts()
    {
        // Arrange
        var query = GenerateValidQuery();
        var testSales = CreateTestSales(2);
        var expectedTotalRevenue = testSales.Sum(s => s.TotalPrice);
        var expectedProductRevenues = testSales
            .SelectMany(s => s.Products)
            .GroupBy(p => p.Id)
            .Select(g => new ProductRevenue(g.Key, g.First().Name, g.Sum(p => p.Price * p.Quantity)))
            .ToList();

        _saleRepoMock.Setup(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination))
            .ReturnsAsync((testSales, testSales.Count, expectedTotalRevenue));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalSalesCount.Should().Be(testSales.Count);
        result.TotalRevenue.Should().Be(expectedTotalRevenue);
        result.ProductRevenues.Should().BeEquivalentTo(expectedProductRevenues);
        _saleRepoMock.Verify(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnZeroMetrics_WhenNoSalesInPeriod()
    {
        // Arrange
        var query = GenerateValidQuery();
        var emptySales = new List<Sale>();

        _saleRepoMock.Setup(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination))
            .ReturnsAsync((emptySales, 0, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalSalesCount.Should().Be(0);
        result.TotalRevenue.Should().Be(0);
        result.ProductRevenues.Should().BeEmpty();
        _saleRepoMock.Verify(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyProductRevenues_WhenSalesHaveNoProducts()
    {
        // Arrange
        var query = GenerateValidQuery();
        var salesWithoutProducts = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.Products, [])
            .Generate(2);

        _saleRepoMock.Setup(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination))
            .ReturnsAsync((salesWithoutProducts, salesWithoutProducts.Count, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalSalesCount.Should().Be(2);
        result.TotalRevenue.Should().Be(0);
        result.ProductRevenues.Should().BeEmpty();
    }
}
