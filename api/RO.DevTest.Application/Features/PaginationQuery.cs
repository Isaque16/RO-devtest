namespace RO.DevTest.Application.Features;

public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool isAscend { get; set; } = false;
}
