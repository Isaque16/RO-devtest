using MediatR;

namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

public class GetAllSalesByPeriodQuery
    : IRequest<GetAllSalesByPeriodResult>
{
    public DateTimeRange DateRange { get; set; } = new(DateTime.Now, DateTime.Now); // Intervalo de datas
    
    public PaginationQuery Pagination { get; set; } = new(); // Paginação
}

public record DateTimeRange(DateTime StartDate, DateTime EndDate);
