using Moq;
using Bogus;
using FluentAssertions;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;
using RO.DevTest.Domain.Entities.ReducedEntities;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

using FluentValidation;
using Domain.Entities;

public class CreateSaleCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new CreateSaleCommandHandler(_saleRepoMock.Object);
    }
    
    private static CreateSaleCommand GenerateValidCommand()
    {
        return new Faker<CreateSaleCommand>()
            .RuleFor(c => c.Products, f => f.Make(3, () => new RProduct()
            {
                Id = f.Random.Guid(),
                Name = f.Commerce.ProductName(),
                Description = f.Lorem.Sentence(),
                Price = f.Random.Decimal(1, 1000),
                Quantity = f.Random.Int(1, 100),
                ImageUrl = f.Internet.Url()
            }))
            .RuleFor(c => c.CustomerId, f => f.Random.String(10))
            .Generate();
    }
    
    [Fact]
    public async Task Handle_ShouldCreateSale_WhenCommandIsValid()
    {
        // Arrange
        var command = GenerateValidCommand();
        var sale = new Faker<Sale>()
            .RuleFor(s => s.Products, command.Products)
            .RuleFor(s => s.CustomerId, command.CustomerId)
            .Generate();
        _saleRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sale);

        var handler = new CreateSaleCommandHandler(_saleRepoMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equivalent(sale, result);
        _saleRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateSaleCommand(); // Invalid command with missing required properties
        var handler = new CreateSaleCommandHandler(_saleRepoMock.Object);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
        _saleRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
