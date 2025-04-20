using MediatR;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommand(string username, string password) : IRequest<LoginResponse> 
{
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}
