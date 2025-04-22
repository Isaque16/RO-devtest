namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

using FluentValidation;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
  public CreateSaleCommandValidator()
  {
    RuleFor(x => x.Products)
      .NotEmpty()
      .WithMessage("Products cannot be empty.");

    RuleFor(x => x.CustomerId)
      .NotEmpty()
      .WithMessage("Customer ID cannot be empty.");
  }
}
