using RO.DevTest.Domain.Enums;

namespace RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;

using MediatR;
using Contracts.Persistance.Repositories;

/// <summary>
/// Handles the "Get User By Id" query to fetch user details by their unique identifier.
/// </summary>
/// <remarks>
/// This handler uses <see cref="IUserRepository"/> to retrieve a user entity by the
/// provided identifier and maps the entity to the <see cref="GetUserResult"/> record.
/// </remarks>
public class GetUserByIdQueryHandler(IUserRepository userRepo) : 
  IRequestHandler<GetUserByIdQuery, GetUserResult>
{
  /// <summary>
  /// Handles the "Get User By Id" query to retrieve user details by their unique identifier.
  /// </summary>
  /// <param name="request">The query containing the identifier of the user to fetch.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
  /// <returns>A <see cref="GetUserResult"/> containing the user's details.</returns>
  /// <exception cref="KeyNotFoundException">Thrown if no user is found with the specified identifier.</exception>
  public async Task<GetUserResult> Handle(
    GetUserByIdQuery request, CancellationToken cancellationToken)
  {
    var user = await userRepo.GetByIdAsync(request.Id, cancellationToken) ??
               throw new KeyNotFoundException($"Usuário com ID {request.Id} não foi encontrado.");
    
    return new GetUserResult(
      Id: user.Id,
      Name: user.Name,
      UserName: user.UserName!,
      PhoneNumber: user.PhoneNumber!,
      Email: user.Email!,
      Role: UserRoles.Customer
    );
  }
}
