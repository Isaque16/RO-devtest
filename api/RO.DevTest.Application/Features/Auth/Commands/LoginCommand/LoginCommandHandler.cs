using MediatR;
using RO.DevTest.Application.Contracts.Infrastructure;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommandHandler(IIdentityAbstractor identityAbstractor, ITokenService tokenService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo username
        var user = await identityAbstractor.FindUserByUserNameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // Verifica a senha
        var signInResult = await identityAbstractor.PasswordSignInAsync(user, request.Password);
        if (!signInResult.Succeeded) 
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // Obtém os cargos do usuário
        var roles = await identityAbstractor.GetUserRolesAsync(user);

        // Gera os tokens (AccessToken e RefreshToken)
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();

        // Retorna a resposta de login
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = roles,
            ExpirationDate = DateTime.UtcNow.AddHours(1)
        };
    }
}
