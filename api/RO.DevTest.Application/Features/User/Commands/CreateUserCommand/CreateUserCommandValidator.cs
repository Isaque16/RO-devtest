namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

using FluentValidation;

public class CreateUserCommandValidator 
    : AbstractValidator<CreateUserCommand> 
{
    public CreateUserCommandValidator() 
    {
        RuleFor(cpau => cpau.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("O campo nome precisa ser preenchido");
        
        RuleFor(cpau => cpau.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("O campo nome de usuário precisa ser preenchido");
        
        RuleFor(cpau => cpau.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("O campo e-mail precisa ser preenchido");

        RuleFor(cpau => cpau.Password)
            .MinimumLength(6)
            .WithMessage("O campo senha precisa ter, pelo menos, 6 caracteres");
    }
}
