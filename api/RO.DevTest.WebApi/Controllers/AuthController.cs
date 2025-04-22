namespace RO.DevTest.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.Auth.Commands.LoginCommand;
using MediatR;

[Route("api/auth")]
[OpenApiTag("Auth", Description = "Endpoints for authentication.")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Performs user login.
    /// </summary>
    /// <param name="request">Authentication data.</param>
    /// <returns>Authentication token and user information.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
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
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error during login.", Details = ex.Message });
        }
    }
}
