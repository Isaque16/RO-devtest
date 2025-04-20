namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using MediatR;
using Contracts.Persistance.Repositories;
using Domain.Entities;
using RO.DevTest.Domain.Exception;

/// <summary>
/// Handles the execution of the update sale command, which is used to
/// update a <see cref="Sale"/> entity in the data store.
/// </summary>
/// <remarks>
/// This class validates the command, retrieves the existing sale from the repository,
/// applies the updates from the command, and persists the updated sale back to the repository.
/// If the sale is not found or the update operation fails, appropriate exceptions are thrown.
/// </remarks>
/// <param name="saleRepo">
/// An instance of <see cref="ISaleRepository"/>, used to interact with the data store for <see cref="Sale"/> entities.
/// </param>
public class UpdateSaleCommandHandler(ISaleRepository saleRepo) : IRequestHandler<UpdateSaleCommand, Sale>
{
  /// <summary>
  /// Handles the execution of the <see cref="UpdateSaleCommand"/> to update a sale.
  /// </summary>
  /// <param name="request">The <see cref="UpdateSaleCommand"/> containing the details for the sale update.</param>
  /// <param name="cancellationToken">A token to cancel the operation.</param>
  /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation, containing the updated <see cref="Sale"/> entity.</returns>
  /// <exception cref="BadRequestException">Thrown when the command fails validation.</exception>
  /// <exception cref="KeyNotFoundException">Thrown when no sale is found for the specified ID.</exception>
  /// <exception cref="Exception">Thrown when the update operation fails.</exception>
  public async Task<Sale> Handle(UpdateSaleCommand request,
    CancellationToken cancellationToken)
  {
    UpdateSaleCommandValidator validator = new();
    var validationResult =
      await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      throw new BadRequestException(validationResult);

    var sale = await saleRepo.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");
    sale = request.AssignToSale(sale);
    
    return await saleRepo.UpdateAsync(sale) ?? throw new Exception($"Failed to update sale with ID {request.Id}.");
  }
}