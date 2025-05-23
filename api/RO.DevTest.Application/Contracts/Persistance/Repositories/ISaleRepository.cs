using RO.DevTest.Application.Features;
using RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

namespace RO.DevTest.Application.Contracts.Persistance.Repositories;

using Domain.Entities;

public interface ISaleRepository : IBaseRepository<Sale>
{
    public Task<(List<Sale> pagedSales, int totalCount, decimal totalRevenue)> GetAllSalesByPeriodAsync(DateTimeRange dateTimeRange, PaginationQuery pagination);
}
