namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

public record CreateUserResult(
    string Id,
    string Name,
    string UserName,
    string Email
)
{
    public CreateUserResult(Domain.Entities.User user) 
        : this(
            Id: user.Id,
            Name: user.Name!,
            UserName: user.UserName!,
            Email: user.Email!
        ) { }
}