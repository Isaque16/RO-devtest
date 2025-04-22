using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

/// <summary>
/// Handler for updating a product.
/// </summary>
/// <param name="productRepository"></param>
public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, Product>
{
  /// <summary>
  /// Handles the update product command.
  /// Validates the request, retrieves the product by ID, updates it, and returns the updated product.
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns>
  /// Returns the updated product.
  /// </returns>
  /// <exception cref="BadRequestException"></exception>
  /// <exception cref="KeyNotFoundException"></exception>
  /// <exception cref="Exception"></exception>
  public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
  {
    UpdateProductCommandValidator validator = new();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (!validationResult.IsValid) 
      throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);
    
    var product = await productRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
    product = request.AssignTo(product);

    return await productRepository.UpdateAsync(product) ?? throw new Exception($"Failed to update product with ID {request.Id}.");
  }
}
