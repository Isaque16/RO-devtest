namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

using FluentValidation;

public class GetAllSalesByPeriodQueryValidator 
    : AbstractValidator<GetAllSalesByPeriodQuery>
{
    public GetAllSalesByPeriodQueryValidator()
    {
        RuleFor(x => x.DateRange.StartDate)
            .NotEmpty()
            .WithMessage("A data de início não pode ser vazia.")
            .LessThan(x => x.DateRange.EndDate)
            .WithMessage("A data de início deve ser menor que a data de término.");

        RuleFor(x => x.DateRange.EndDate)
            .NotEmpty()
            .WithMessage("A data de término não pode ser vazia.")
            .GreaterThan(x => x.DateRange.StartDate)
            .WithMessage("A data de término deve ser maior que a data de início.");
    }
}
