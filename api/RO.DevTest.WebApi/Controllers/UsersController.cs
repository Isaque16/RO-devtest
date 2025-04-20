namespace RO.DevTest.WebApi.Controllers;

using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.User.Commands.CreateUserCommand;
using Application.Features.User.Commands.DeleteUserCommand;
using Application.Features.User.Queries.GetAllUsersQuery;
using Application.Features.User.Queries.GetUserByIdQuery;

[Route("api/user")]
[OpenApiTags("Users")]
public class UsersController(IMediator mediator) : ControllerBase 
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetAllUserResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(PaginationQuery pagination) 
    {
        var users = await mediator.Send(new GetAllUsersQuery(pagination));
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserByIdResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(string id) 
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser(CreateUserCommand request) 
    {
        var createdUser = await mediator.Send(request);
        return Created(HttpContext.Request.GetDisplayUrl(), createdUser);
    }

    [HttpPut]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser(CreateUserCommand request) 
    {
        var updatedUser = await mediator.Send(request);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(string id) 
    {
        var result = await mediator.Send(new DeleteUserCommand(id));
        return result ? Ok() : NotFound();
    }
}
