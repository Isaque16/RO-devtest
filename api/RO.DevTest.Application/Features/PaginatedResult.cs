namespace RO.DevTest.Application.Features;

using MediatR;

public class PaginatedResult<T>(List<T> content, int totalCount, int pageNumber, int pageSize) : IRequest<PaginatedResult<T>>
{
  public List<T> Content { get; set; } = content;
  public int PageNumber { get; set; } = pageNumber;
  public int PageSize { get; set; } = pageSize;
  public int TotalCount { get; set; } = totalCount;
  public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
