namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

using FluentValidation;

/// <summary>
/// Validator for the CreateProductCommand class.
/// </summary>
public class CreateProductCommandValidator 
  : AbstractValidator<CreateProductCommand>
{
  public CreateProductCommandValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Name is required.");

    RuleFor(x => x.Description)
      .NotEmpty()
      .WithMessage("Description is required.");

    RuleFor(x => x.Price)
      .GreaterThan(0)
      .WithMessage("Price must be greater than 0.");

    RuleFor(x => x.Quantity)
      .GreaterThanOrEqualTo(0)
      .WithMessage("Quantity must be greater than or equal to 0.");

    RuleFor(x => x.ImageUrl)
      .NotEmpty()
      .WithMessage("Image URL is required.")
      .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
      .WithMessage("Image URL must be a valid absolute URL.");
  }
}
