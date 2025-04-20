namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

using FluentValidation;
using MediatR;

/// <summary>
/// Validator for the UpdateProductCommand class.
/// </summary>
public class UpdateProductCommandValidator 
  : AbstractValidator<UpdateProductCommand>
{
  public UpdateProductCommandValidator()
  {
    RuleFor(x => x.Id)
      .NotEmpty()
      .WithMessage("Product ID is required.");

    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Product name is required.");

    RuleFor(x => x.Description)
      .NotEmpty()
      .WithMessage("Product description is required.");

    RuleFor(x => x.Price)
      .GreaterThan(0)
      .WithMessage("Product price must be greater than zero.");

    RuleFor(x => x.Quantity)
      .GreaterThanOrEqualTo(0)
      .WithMessage("Product quantity cannot be negative.");

    RuleFor(x => x.ImageUrl)
      .NotEmpty()
      .WithMessage("Product image URL is required.");
  }
}
