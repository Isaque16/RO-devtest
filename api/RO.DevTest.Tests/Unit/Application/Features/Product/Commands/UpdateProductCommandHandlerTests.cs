using Bogus;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

using Moq;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _handler = new UpdateProductCommandHandler(_productRepoMock.Object);
    }

    private static UpdateProductCommand GenerateValidCommand()
    {
        return new Faker<UpdateProductCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Commerce.ProductName())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence())
            .RuleFor(c => c.Price, f => f.Random.Decimal(1, 1000))
            .RuleFor(c => c.Quantity, f => f.Random.Int(1, 100))
            .RuleFor(c => c.ImageUrl, f => f.Internet.Url())
            .Generate();
    }

    private static Product GenerateExistingProduct(Guid id)
    {
        return new Product
        {
            Id = id,
            Name = "Old Name",
            Price = 50,
            Description = "Old Description",
            Quantity = 10
        };
    }

    [Fact]
    public async Task Handle_UpdatesProductSuccessfully_WhenDataIsValid()
    {
        // Arrange
        var command = GenerateValidCommand();
        var existingProduct = GenerateExistingProduct(command.Id);
        var updatedProduct = command.AssignTo(existingProduct);

        _productRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);
        _productRepoMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        Assert.Equal(result.Id, command.Id);
        Assert.Equal(result.Name, command.Name);
        Assert.Equal(result.Description, command.Description);
        Assert.Equal(result.Price, command.Price);
        Assert.Equal(result.Quantity, command.Quantity);
        Assert.Equal(result.ImageUrl, command.ImageUrl);
    }

    [Fact]
    public async Task Handle_ThrowsValidationException_WhenCommandIsInvalid()
    {
        // Arrange  
        var command = new UpdateProductCommand(); // Dados inválidos

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*validation errors occurred*");
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenProductDoesNotExist()
    {
        // Arrange
        var command = GenerateValidCommand();

        _productRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with ID {command.Id} not found.");
    }
}
