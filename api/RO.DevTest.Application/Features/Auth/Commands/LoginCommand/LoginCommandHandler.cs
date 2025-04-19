using MediatR;
using RO.DevTest.Application.Contracts.Infrastructure;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommandHandler(IIdentityAbstractor identityAbstractor, ITokenService tokenService) : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IIdentityAbstractor _identityAbstractor = identityAbstractor;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo username
        var user = await _identityAbstractor.FindUserByUserNameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // Verifica a senha
        var signInResult = await _identityAbstractor.PasswordSignInAsync(user, request.Password);
        if (!signInResult.Succeeded) 
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        // Obtém os cargos do usuário
        var roles = await _identityAbstractor.GetUserRolesAsync(user);

        // Gera os tokens (AccessToken e RefreshToken)
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

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
