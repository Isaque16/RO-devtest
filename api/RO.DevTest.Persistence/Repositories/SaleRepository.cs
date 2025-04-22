namespace RO.DevTest.Persistence.Repositories;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Features;
using Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

public class SaleRepository(DefaultContext context)
    : BaseRepository<Sale>(context), ISaleRepository
{
    public async Task<(List<Sale> pagedSales, int totalCount, decimal totalRevenue)> GetAllSalesByPeriodAsync(
        DateTimeRange dateTimeRange, PaginationQuery pagination)
    {
        var baseQuery = Context.Set<Sale>()
            .Where(sale => sale.CreatedOn >= dateTimeRange.StartDate && 
                           sale.CreatedOn <= dateTimeRange.EndDate)
            .Include(sale => sale.Products);

        var totalCount = await baseQuery.CountAsync();
        var totalRevenue = await baseQuery.SumAsync(s => s.TotalPrice);

        var pagedSales = await baseQuery
            .OrderBy(sale => sale.CreatedOn)
            .Skip(Math.Max(0, (pagination.PageNumber - 1)) * Math.Max(1, pagination.PageSize))
            .Take(Math.Max(1, pagination.PageSize))
            .ToListAsync();

        return (pagedSales, totalCount, totalRevenue);
    }
}
