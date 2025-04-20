namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;
using MediatR;

// Query para obter todos os usuários
public class GetAllUsersQuery(PaginationQuery pagination) : IRequest<PaginatedResult<GetAllUserResult>> {
    public PaginationQuery Pagination { get; set; } = pagination;
}
