namespace RO.DevTest.WebApi.Controllers;

using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.User.Queries;
using Application.Features.User.Commands.CreateUserCommand;
using Application.Features.User.Commands.DeleteUserCommand;
using Application.Features.User.Queries.GetAllUsersQuery;
using Application.Features.User.Queries.GetUserByIdQuery;

[Route("api/user")]
[OpenApiTag("Users", Description = "Endpoints para gerenciar usuários.")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Obtém todos os usuários com paginação.
    /// </summary>
    /// <param name="pagination">Parâmetros de paginação.</param>
    /// <returns>Lista paginada de usuários.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetUserResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginationQuery pagination)
    {
        try
        {
            var users = await mediator.Send(new GetAllUsersQuery(pagination));
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Erro ao buscar usuários.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>O usuário correspondente ao ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var user = await mediator.Send(new GetUserByIdQuery(id));
            if (user == null)
                return NotFound(new { Message = "Usuário não encontrado." });
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Erro ao buscar o usuário.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="request">Dados do usuário a ser criado.</param>
    /// <returns>O usuário criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
    {
        try
        {
            var createdUser = await mediator.Send(request);
            return Created(HttpContext.Request.GetDisplayUrl(), createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Erro ao criar o usuário.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    /// <param name="request">Dados do usuário a ser atualizado.</param>
    /// <returns>O usuário atualizado.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] CreateUserCommand request)
    {
        try
        {
            var updatedUser = await mediator.Send(request);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Erro ao atualizar o usuário.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Deleta um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser deletado.</param>
    /// <returns>Confirmação da exclusão.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var result = await mediator.Send(new DeleteUserCommand(id));
            return result ? Ok() : NotFound(new { Message = "Usuário não encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "Erro ao deletar o usuário.", Details = ex.Message });
        }
    }
}
