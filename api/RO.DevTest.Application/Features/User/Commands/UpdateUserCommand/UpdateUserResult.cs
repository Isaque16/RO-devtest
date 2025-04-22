using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

public record UpdateUserResult(
  string Id,
  string Name,
  string UserName,
  string PhoneNumber,
  string Email,
  UserRoles Role
);
