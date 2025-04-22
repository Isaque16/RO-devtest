namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;

using MediatR;

// Query para obter todos os usuários
public class GetAllUsersQuery
    : IRequest<PaginatedResult<GetUserResult>> {
    public PaginationQuery Pagination { get; set; } = new();
}
