namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

public record CreateUserResult(
    string Id,
    string UserName,
    string Name,
    string Email
)
{
    public CreateUserResult(Domain.Entities.User user) 
        : this(
            Id: user.Id,
            UserName: user.UserName!,
            Name: user.Name!,
            Email: user.Email!
        ) { }
}