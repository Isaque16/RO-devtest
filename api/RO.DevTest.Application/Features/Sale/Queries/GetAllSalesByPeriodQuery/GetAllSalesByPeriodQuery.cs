using MediatR;

namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

public class GetAllSalesByPeriodQuery(DateTimeRange dateTimeRange, PaginationQuery pagination)
    : IRequest<GetAllSalesByPeriodResult>
{
    public DateTimeRange DateRange { get; set; } = dateTimeRange; // Intervalo de datas
    
    public PaginationQuery Pagination { get; set; } = pagination; // Paginação
}

public record DateTimeRange(DateTime StartDate, DateTime EndDate);
