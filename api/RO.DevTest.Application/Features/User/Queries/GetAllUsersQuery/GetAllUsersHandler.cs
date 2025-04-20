namespace RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;

using MediatR;
using Contracts.Persistance.Repositories;

/// <summary>
/// Handles the query to retrieve paginated results of all users.
/// </summary>
public class GetAllUsersQueryHandler(IUserRepository userRepo) 
  : IRequestHandler<GetAllUsersQuery, PaginatedResult<GetUserResult>>
{
  /// <summary>
  /// Handles the request to retrieve a paginated list of users.
  /// </summary>
  /// <param name="request">The query request containing pagination details.</param>
  /// <param name="cancellationToken">The cancellation token to cancel the operation, if needed.</param>
  /// <returns>A task representing the asynchronous operation. The task result contains a paginated result of users as DTOs.</returns>
  public async Task<PaginatedResult<GetUserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
  {
    var users = await userRepo.GetAllPagedAsync(request.Pagination, cancellationToken);
    
    // Map the user entities to the GetAllUserResult DTOs
    var usersContent = users.Content.Select(user => new GetUserResult
    (
      Id: user.Id,
      Name: user.Name,
      Email: user.Email!
    )).ToList();
    
    return new PaginatedResult<GetUserResult>(usersContent, users.TotalCount, users.PageNumber, users.PageSize);
  }
}
