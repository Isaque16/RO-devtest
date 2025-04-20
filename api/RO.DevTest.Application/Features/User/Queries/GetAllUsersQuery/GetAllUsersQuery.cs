namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;
using MediatR;

// Query para obter todos os usuários
public class GetAllUsersQuery(int pageNumber = 1, int pageSize = 10) : IRequest<PaginatedResult<GetAllUserResult>> {
  public int PageNumber { get; set; } = pageNumber;
  
  public int PageSize { get; set; } = pageSize;
}
