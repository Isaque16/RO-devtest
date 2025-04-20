namespace RO.DevTest.Application.Features.User.Queries;

public record GetUserResult(
    string Id,
    string Name,
    string Email
);
