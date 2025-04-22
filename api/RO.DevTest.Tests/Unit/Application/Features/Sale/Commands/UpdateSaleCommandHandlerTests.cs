using Bogus;
using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;
using RO.DevTest.Domain.Entities.ReducedEntities;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

using Domain.Entities;
using FluentValidation;

public class UpdateSaleCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly UpdateSaleCommandHandler _handler;

    public UpdateSaleCommandHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new UpdateSaleCommandHandler(_saleRepoMock.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldUpdateSale_WhenCommandIsValid()
    {
        // Arrange
        var command = new Faker<UpdateSaleCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
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

        var existingSale = new Faker<Sale>()
            .RuleFor(s => s.Id, command.Id)
            .RuleFor(s => s.Products, f => f.Make(2, () => new RProduct()))
            .RuleFor(s => s.CustomerId, f => f.Random.String(10))
            .Generate();

        var updatedSale = command.AssignTo(existingSale);

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);
        _saleRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Sale>()))
            .ReturnsAsync(updatedSale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equivalent(updatedSale, result);
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Sale>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new UpdateSaleCommand(); // Invalid command with missing required properties

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _saleRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Sale>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenSaleDoesNotExist()
    {
        // Arrange
        var command = new Faker<UpdateSaleCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Products, f => f.Make(3, () => new RProduct()))
            .RuleFor(c => c.CustomerId, f => f.Random.String(10))
            .Generate();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sale)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found.");
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Sale>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUpdateFails()
    {
        // Arrange
        var command = new Faker<UpdateSaleCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Products, f => f.Make(3, () => new RProduct()))
            .RuleFor(c => c.CustomerId, f => f.Random.String(10))
            .Generate();

        var existingSale = new Faker<Sale>()
            .RuleFor(s => s.Id, command.Id)
            .RuleFor(s => s.Products, f => f.Make(2, () => new RProduct()))
            .RuleFor(s => s.CustomerId, f => f.Random.String(10))
            .Generate();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);
        _saleRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Sale>()))
            .ReturnsAsync((Sale)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Failed to update sale with ID {command.Id}.");
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Sale>()), Times.Once);
    }
}
