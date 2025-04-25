namespace RO.DevTest.Application.Features;

using MediatR;

/// <summary>
/// Represents a result set that is paginated, including the content, page information,
/// and metadata such as total count and total pages.
/// </summary>
/// <typeparam name="T">The type of elements in the paginated result.</typeparam>
public record PaginatedResult<T>(
  List<T> Content,
  int TotalCount,
  int PageNumber,
  int PageSize)
{
  public List<T> Content { get; set; } = Content;
  public int PageNumber { get; set; } = PageNumber;
  public int PageSize { get; set; } = PageSize;
  public int TotalCount { get; set; } = TotalCount;
  public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
