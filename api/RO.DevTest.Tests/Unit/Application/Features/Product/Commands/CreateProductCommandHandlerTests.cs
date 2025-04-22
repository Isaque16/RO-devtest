using Bogus;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

using Moq;
using Domain.Entities;
using FluentValidation;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _handler = new CreateProductCommandHandler(_productRepoMock.Object);
    }

    private static CreateProductCommand GenerateValidCommand()
    {
        return new Faker<CreateProductCommand>()
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
            .RuleFor(c => c.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(c => c.ImageUrl, f => f.Internet.Url())
            .Generate();
    }

    [Fact]
    public async Task Handle_ShouldCreateProduct_WhenRequestIsValid()
    {
        // Arrange
        var command = GenerateValidCommand();

        _productRepoMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(command.AssignTo());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Price, result.Price);
        Assert.Equal(command.Quantity, result.Quantity);
        Assert.Equal(command.ImageUrl, result.ImageUrl);
        _productRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenRequestIsInvalid()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "", Price = -1 }; // Invalid data

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("validation errors occurred", exception.Message);
        _productRepoMock.Verify(repo => repo.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
