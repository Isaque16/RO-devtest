namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

using Bogus;
using Moq;
using Domain.Entities;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;

public class DeleteSaleCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTests()
    {
        _saleRepoMock = new Mock<ISaleRepository>();
        _handler = new DeleteSaleCommandHandler(_saleRepoMock.Object);
    }
    
    private static DeleteSaleCommand GenerateValidCommand()
    {
        return new Faker<DeleteSaleCommand>()
            .CustomInstantiator(f => new DeleteSaleCommand(f.Random.Guid()));
    }
    
    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenSaleIsSuccessfullyDeleted()
    {
        // Arrange
        var command = GenerateValidCommand();

        var existingSale = new Faker<Sale>()
            .RuleFor(s => s.Id, command.Id)
            .Generate();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);
        _saleRepoMock.Setup(repo => repo.DeleteAsync(existingSale))
            .ReturnsAsync(true);


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.DeleteAsync(existingSale), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenSaleDoesNotExist()
    {
        // Arrange
        var command = GenerateValidCommand();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Sale)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Sale>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenSaleDeletionFails()
    {
        // Arrange
        var command = GenerateValidCommand();

        var existingSale = new Faker<Sale>()
            .RuleFor(s => s.Id, command.Id)
            .Generate();

        _saleRepoMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);
        _saleRepoMock.Setup(repo => repo.DeleteAsync(existingSale))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _saleRepoMock.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _saleRepoMock.Verify(repo => repo.DeleteAsync(existingSale), Times.Once);
    }
}
