using MediatR;
using RO.DevTest.Application.Contracts.Infrastructure;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

/// <summary>
/// Handles the login command by validating user credentials, retrieving user roles,
/// and generating access and refresh tokens.
/// </summary>
/// <remarks>
/// This class manages the user authentication process. It uses dependency-injected services
/// such as <see cref="IIdentityAbstractor"/> for user management and <see cref="ITokenService"/>
/// for token generation.
/// </remarks>
/// <param name="identityAbstractor">Service for interacting with the identity system to manage users.</param>
/// <param name="tokenService">Service for generating authentication tokens.</param>
/// <seealso cref="IIdentityAbstractor"/>
/// <seealso cref="ITokenService"/>
public class LoginCommandHandler(
    IIdentityAbstractor identityAbstractor, ITokenService tokenService) 
    : IRequestHandler<LoginCommand, LoginResponse>
{
    /// <summary>
    /// Handles the login command by validating user credentials, retrieving user roles, and generating access and refresh tokens.
    /// </summary>
    /// <param name="request">The login command request containing the username and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="LoginResponse"/> containing the access token, refresh token, user roles, and token expiration date.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the provided username or password is invalid.</exception>
    public async Task<LoginResponse> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo username
        var user =
            await identityAbstractor.FindUserByUserNameAsync(request.Username)
            ?? throw new UnauthorizedAccessException(
                "Usuário ou senha inválidos.");

        // Verifica a senha
        var signInResult =
            await identityAbstractor.PasswordSignInAsync(user,
                request.Password);
        if (!signInResult.Succeeded)
            throw new UnauthorizedAccessException(
                "Usuário ou senha inválidos.");

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
