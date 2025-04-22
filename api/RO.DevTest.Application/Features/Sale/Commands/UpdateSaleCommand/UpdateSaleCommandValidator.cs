namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using FluentValidation;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
  public UpdateSaleCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    RuleFor(x => x.Products).NotEmpty().WithMessage("Products are required.");
    RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
  }
}
