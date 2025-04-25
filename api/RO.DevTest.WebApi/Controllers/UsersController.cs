using RO.DevTest.Application.Features.User.Commands.UpdateUserCommand;

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

/// <summary>
/// Controller responsible for managing users.
/// </summary>
[Route("api/users")]
[OpenApiTag("Users", Description = "Endpoints to manage users.")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves all users with pagination.
    /// </summary>
    /// <param name="pagination">Pagination parameters.</param>
    /// <returns>A paginated list of users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetUserResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery pagination)
    {
        try
        {
            var users = await mediator.Send(pagination);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error retrieving users.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user corresponding to the given ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await mediator.Send(query);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error retrieving the user.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="newUser">Data of the user to be created.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand newUser)
    {
        try
        {
            var createdUser = await mediator.Send(newUser);
            return Created(HttpContext.Request.GetDisplayUrl(), createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error creating the user.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">Data of the user to be updated.</param>
    /// <returns>The updated user.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand user)
    {
        try
        {
            var updatedUser = await mediator.Send(user);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error updating the user.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to be deleted.</param>
    /// <returns>Confirmation of the deletion.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var command = new DeleteUserCommand(id);
            var result = await mediator.Send(command);
            return result ? Ok() : NotFound(new { Message = "User not found." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Error deleting the user.", Details = ex.Message });
        }
    }
}
