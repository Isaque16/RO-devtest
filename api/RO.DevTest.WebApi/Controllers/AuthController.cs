namespace RO.DevTest.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.Auth.Commands.LoginCommand;
using MediatR;

[Route("api/auth")]
[OpenApiTag("Auth", Description = "Endpoints para autenticação.")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Realiza o login de um usuário.
    /// </summary>
    /// <param name="request">Dados para autenticação.</param>
    /// <returns>Token de autenticação e informações do usuário.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        try
        {
            var response = await mediator.Send(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao realizar login.", Details = ex.Message });
        }
    }
}
