namespace RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;

using MediatR;
using Contracts.Persistance.Repositories;
using Domain.Entities;

/// <summary>
/// Handles the query to retrieve all sales with pagination support.
/// </summary>
/// <remarks>
/// Retrieves a paginated result of sales entities by using the ISaleRepository interface.
/// This handler processes the <see cref="GetAllSalesQuery"/> request and returns a <see cref="PaginatedResult{T}"/>
/// containing a list of sales and associated metadata such as total count, page number, and page size.
/// </remarks>
/// <param name="saleRepository">
/// Injected instance of <see cref="ISaleRepository"/> used to access sales data from the underlying data source.
/// </param>
public class GetAllSalesQueryHandler(ISaleRepository saleRepository) : IRequestHandler<GetAllSalesQuery, PaginatedResult<Sale>>
{
  /// <summary>
  /// Handles the execution of the <see cref="GetAllSalesQuery"/> to retrieve a paginated list of sales.
  /// </summary>
  /// <param name="request">
  /// An instance of <see cref="GetAllSalesQuery"/> containing the pagination parameters for retrieving sales.
  /// </param>
  /// <param name="cancellationToken">
  /// A token to monitor for cancellation requests.
  /// </param>
  /// <returns>
  /// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedResult{Sale}"/>
  /// with a list of sales and pagination metadata including total count, page number, and page size.
  /// </returns>
  public async Task<PaginatedResult<Sale>> Handle(GetAllSalesQuery request,
    CancellationToken cancellationToken)
  {
    var sales = await saleRepository.GetAllPagedAsync(request.Pagination, cancellationToken);
    return new PaginatedResult<Sale>(sales.Content, sales.TotalCount, sales.PageNumber, sales.PageSize);
  }
}
