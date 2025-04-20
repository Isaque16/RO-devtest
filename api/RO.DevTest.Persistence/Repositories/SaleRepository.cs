namespace RO.DevTest.Persistence.Repositories;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Features;
using Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

public class SaleRepository(DefaultContext context)
    : BaseRepository<Sale>(context), ISaleRepository
{
    public async Task<List<Sale>> GetAllSalesByPeriodAsync(DateTimeRange dateTimeRange, PaginationQuery pagination)
    {
        return await Context.Set<Sale>()
            .Where(sale =>
                sale.CreatedOn >= dateTimeRange.StartDate && sale.CreatedOn <= dateTimeRange.EndDate)
            .OrderBy(sale => sale.CreatedOn) // Ordenação para consistência
            .Skip(Math.Max(0, (pagination.PageNumber - 1)) * Math.Max(1, pagination.PageSize))
            .Take(Math.Max(1, pagination.PageSize))
            .Select(sale => new Sale
            {
                Id = sale.Id,
                CreatedOn = sale.CreatedOn,
                TotalPrice = sale.TotalPrice,
                Quantity = sale.Quantity,
                CustomerId = sale.CustomerId
            })
            .ToListAsync();
    }
}
