namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;

public record GetAllUserResult
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
