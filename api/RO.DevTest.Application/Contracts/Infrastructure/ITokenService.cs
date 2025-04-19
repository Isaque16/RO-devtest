using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Infrastructure;

public interface ITokenService
{
  string GenerateAccessToken(User user, IList<string> roles);
  string GenerateRefreshToken();
}