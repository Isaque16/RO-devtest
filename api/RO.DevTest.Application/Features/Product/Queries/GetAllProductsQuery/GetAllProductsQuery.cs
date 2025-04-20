namespace RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;

using MediatR;
using RO.DevTest.Application.Features.User.Queries;

/// <summary>
/// Query to get all products with pagination
/// </summary>
/// <param name="pageNumber"></param>
/// <param name="pageSize"></param>
public class GetAllProductsQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PaginatedResult<Domain.Entities.Product>> { 
  public int PageNumber { get; set; } = pageNumber;
  
  public int PageSize { get; set; } = pageSize;
}
