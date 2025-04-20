namespace RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;

using FluentValidation;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
  public UpdateSaleCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    RuleFor(x => x.Products).NotEmpty().WithMessage("Products are required.");
    RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    RuleFor(x => x.TotalPrice).GreaterThan(0).WithMessage("TotalPrice must be greater than 0.");
    RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    RuleFor(x => x.Customer).NotNull().WithMessage("Customer is required.");
  }
}
