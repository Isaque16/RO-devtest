using RO.DevTest.Application.Contracts.Persistance.Repositories;

using MediatR;

namespace RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
  public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await userRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Usuário com ID {request.Id} não foi encontrado.");
    
    return new GetUserByIdResult {
      Id = user.Id,
      Name = user.Name ?? string.Empty,
      Email = user.Email ?? string.Empty,
    };
  }
}
