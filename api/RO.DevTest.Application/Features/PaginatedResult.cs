namespace RO.DevTest.Application.Features;

using MediatR;

/// <summary>
/// Represents a result set that is paginated, including the content, page information,
/// and metadata such as total count and total pages.
/// </summary>
/// <typeparam name="T">The type of elements in the paginated result.</typeparam>
public record PaginatedResult<T>(
  List<T> content,
  int totalCount,
  int pageNumber,
  int pageSize)
{
  public List<T> Content { get; set; } = content;
  public int PageNumber { get; set; } = pageNumber;
  public int PageSize { get; set; } = pageSize;
  public int TotalCount { get; set; } = totalCount;
  public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
