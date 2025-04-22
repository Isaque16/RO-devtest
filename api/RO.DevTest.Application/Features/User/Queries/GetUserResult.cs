using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Application.Features.User.Queries;

public record GetUserResult(
    string Id,
    string Name,
    string UserName,
    string Email,
    string PhoneNumber,
    UserRoles Role
);
