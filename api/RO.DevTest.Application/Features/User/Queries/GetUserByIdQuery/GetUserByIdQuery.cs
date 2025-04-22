using MediatR;

namespace RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;

public class GetUserByIdQuery : IRequest<GetUserResult>
{
  public string Id { get; set; } = string.Empty;
}
