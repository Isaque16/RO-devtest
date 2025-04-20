using FluentValidation;

namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using MediatR;
using Contracts.Persistance.Repositories;
using Domain.Entities;
using RO.DevTest.Domain.Exception;

/// <summary>
/// Handles the processing of the <see cref="CreateSaleCommand"/> and is responsible for
/// creating a new <see cref="Sale"/> based on the provided command data.
/// </summary>
/// <remarks>
/// This handler is used to validate the incoming command, transform it into a Sale entity,
/// and persist it to the underlying data source using the <see cref="ISaleRepository"/>.
/// </remarks>
public class CreateSaleCommandHandler(ISaleRepository saleRepo) : IRequestHandler<CreateSaleCommand, Sale>
{
  /// <summary>
  /// Handles the CreateSaleCommand by creating a new Sale entity based on the provided data
  /// and persisting it to the database.
  /// </summary>
  /// <param name="request">The CreateSaleCommand containing all the data required to create a Sale entity.</param>
  /// <param name="cancellationToken">A cancellation token that can be used to signal the operation should be aborted.</param>
  /// <returns>Returns the newly created Sale entity.</returns>
  /// <exception cref="BadRequestException">
  /// Thrown when the provided CreateSaleCommand fails validation.
  /// </exception>
  public async Task<Sale> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
  {
    CreateSaleCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);
    
    var sale = request.AssignTo();

    return await saleRepo.CreateAsync(sale, cancellationToken);
  }
}
