namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;

using MediatR;
using FluentValidation;
using Contracts.Persistance.Repositories;

/// <summary>
/// Handles the query to retrieve all sales within a specified period, applying
/// optional pagination and returning the total sales count, total revenue, and revenue
/// breakdown by product.
/// </summary>
/// <remarks>
/// This query handler leverages the repository pattern via the <see cref="ISaleRepository"/> to access sales data
/// and processes the results to calculate aggregated metrics such as total revenue and product-specific revenues.
/// It also ensures the validation of the query request using the <see cref="GetAllSalesByPeriodQueryValidator"/>.
/// </remarks>
/// <param name="saleRepo">
/// The repository responsible for providing access to sales data.
/// </param>
/// <exception cref="ValidationException">
/// Thrown when the query request is invalid based on the query validator.
/// </exception>
/// <seealso cref="GetAllSalesByPeriodQuery"/>
/// <seealso cref="GetAllSalesByPeriodResult"/>
/// <seealso cref="ProductRevenue"/>
public class GetAllSalesByPeriodQueryHandler(ISaleRepository saleRepo)
    : IRequestHandler<GetAllSalesByPeriodQuery, GetAllSalesByPeriodResult>
{
    /// <summary>
    /// Handles the request to retrieve all sales within a specific time period.
    /// </summary>
    /// <param name="request">The query containing the start date, end date, and pagination details for sales retrieval.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A result containing the total sales count, total revenue, and a list of product revenues within the specified period.</returns>
    /// <exception cref="ValidationException">Thrown when the validation of the query fails.</exception>
    public async Task<GetAllSalesByPeriodResult> Handle(
        GetAllSalesByPeriodQuery request, CancellationToken cancellationToken)
    {
        GetAllSalesByPeriodQueryValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException($"{validationResult.Errors.Count} validation errors occurred.", validationResult.Errors);
        
        var sales = await saleRepo.GetAllSalesByPeriodAsync(request.DateRange, request.Pagination);

        var totalSalesCount = sales.Count;
        var totalRevenue = sales.Sum(s => s.TotalPrice);
        var productRevenues = sales
            .GroupBy(s => s.Id)
            .Select(g => new ProductRevenue(
                g.Key,
                "Unknown Product", // Placeholder for ProductName
                g.Sum(s => s.TotalPrice)
            ))
            .ToList();

        return new GetAllSalesByPeriodResult(totalSalesCount, totalRevenue, productRevenues);
    }
}
