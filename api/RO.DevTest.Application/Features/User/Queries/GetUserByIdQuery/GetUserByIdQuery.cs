using MediatR;

namespace RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;

public class GetUserByIdQuery(string id) : IRequest<GetUserByIdResult>
{
  public string Id { get; set; } = id;
}
