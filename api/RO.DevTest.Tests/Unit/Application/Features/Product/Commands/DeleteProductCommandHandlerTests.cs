using Bogus;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

using Moq;
using Domain.Entities;
using FluentAssertions;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _handler = new DeleteProductCommandHandler(_productRepoMock.Object);
    }

    private static DeleteProductCommand GenerateValidCommand()
    {
        return new Faker<DeleteProductCommand>()
            .CustomInstantiator(f => new DeleteProductCommand(f.Random.Guid()))
            .Generate();
    }

    [Fact]
    public async Task Handle_DeletesProductSuccessfully_WhenProductExists()
    {
        // Arrange
        var command = GenerateValidCommand();

        _productRepoMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = command.Id });

        _productRepoMock
            .Setup(repo => repo.DeleteAsync(It.IsAny<Product>()))
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _productRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Product>()), Times.Once);
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
