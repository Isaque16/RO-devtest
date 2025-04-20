namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

using FluentValidation;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
  public UpdateUserCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Email is not valid.");
  }
}
