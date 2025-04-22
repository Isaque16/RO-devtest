using Bogus;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Queries;

using Moq;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using RO.DevTest.Application.Features;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

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
            });
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSalesWithAggregatedMetrics_WhenValidRequestIsProvided()
    {
        var query = GenerateValidQuery();

        var sales = new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.TotalPrice, f => f.Random.Decimal(10, 1000))
            .Generate(5);

        _saleRepoMock.Setup(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination))
            .ReturnsAsync(sales);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalSalesCount.Should().Be(sales.Count);
        result.TotalRevenue.Should().Be(sales.Sum(s => s.TotalPrice));
        result.ProductRevenues.Should().HaveCount(sales.GroupBy(s => s.Id).Count());
        _saleRepoMock.Verify(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenRequestIsInvalid()
    {
        // Invalid query with the wrong date range
        var query = new Faker<GetAllSalesByPeriodQuery>()
            .RuleFor(s => s.DateRange,
                f => new DateTimeRange(f.Date.Recent(), f.Date.Past()))
            .RuleFor(s => s.Pagination, f => new PaginationQuery
            {
                PageNumber = f.Random.Int(1, 10),
                PageSize = f.Random.Int(1, 50)
            });

        var act = async () => await _handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
        _saleRepoMock.Verify(repo => repo.GetAllSalesByPeriodAsync(It.IsAny<DateTimeRange>(), It.IsAny<PaginationQuery>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoSalesExistInPeriod()
    {
        var query = GenerateValidQuery();

        _saleRepoMock.Setup(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination))
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalSalesCount.Should().Be(0);
        result.TotalRevenue.Should().Be(0);
        result.ProductRevenues.Should().BeEmpty();
        _saleRepoMock.Verify(repo => repo.GetAllSalesByPeriodAsync(query.DateRange, query.Pagination), Times.Once);
    }
}
