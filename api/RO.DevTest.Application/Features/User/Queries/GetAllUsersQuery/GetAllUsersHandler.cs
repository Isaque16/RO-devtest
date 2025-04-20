using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;

// Handler para processar a query
public class GetAllUsersQueryHandler(IUserRepository userRepo) : IRequestHandler<GetAllUsersQuery, PaginatedResult<GetAllUserResult>>
{
  public async Task<PaginatedResult<GetAllUserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
  {
    // Busca os usuários paginados no banco de dados
    var (users, totalCount) = await userRepo.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

    // Mapeia os usuários para o resultado da query
    var result = users.Select(user => new GetAllUserResult
    {
      Id = user.Id,
      Name = user.Name ?? string.Empty,
      Email = user.Email ?? string.Empty,
    }).ToList();

    // Retorna o resultado paginado
    return new PaginatedResult<GetAllUserResult>(result, totalCount, request.PageNumber, request.PageSize);
  }
}
