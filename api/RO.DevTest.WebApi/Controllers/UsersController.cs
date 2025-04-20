using RO.DevTest.Application.Features;

namespace RO.DevTest.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.User.Commands.CreateUserCommand;
using RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Application.Features.User.Queries.GetAllUsersQuery;
using RO.DevTest.Application.Features.User.Queries.GetUserByIdQuery;

[Route("api/user")]
[OpenApiTags("Users")]
public class UsersController(IMediator mediator) : ControllerBase 
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetAllUserResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers() 
    {
        PaginatedResult<GetAllUserResult> users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserByIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(string id) 
    {
        GetUserByIdResult user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(CreateUserCommand request) 
    {
        CreateUserResult createdUser = await _mediator.Send(request);
        return Created(HttpContext.Request.GetDisplayUrl(), createdUser);
    }

    [HttpPut]
    [ProducesResponseType(typeof(CreateUserResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(CreateUserCommand request) 
    {
        CreateUserResult updatedUser = await _mediator.Send(request);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string id) 
    {
        bool result = await _mediator.Send(new DeleteUserCommand(id));
        return result ? Ok() : NotFound();
    }
}
