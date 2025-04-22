namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

using Contracts.Persistance.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

/// <summary>
/// Handles the creation of a new <see cref="Product"/>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CreateProductCommandHandler"/> class.
/// </remarks>
/// <param name="productRepo">The repository for managing products.</param>
public class CreateProductCommandHandler(IProductRepository productRepo) 
  : IRequestHandler<CreateProductCommand, Product>
{
  /// <summary>
  /// Handles the creation of a new product.
  /// </summary>
  /// <param name="request">The command containing the product details.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
  /// <returns>The result of the product creation.</returns>
  public async Task<Product> Handle(
    CreateProductCommand request, CancellationToken cancellationToken)
  {
    CreateProductCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid)
      throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);
    
    var product = request.AssignTo();
    return await productRepo.CreateAsync(product, cancellationToken);
  }
}
