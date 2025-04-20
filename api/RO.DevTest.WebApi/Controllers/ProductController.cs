using RO.DevTest.Application.Features;

namespace RO.DevTest.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Product.Queries.GetAllProductsQuery;
using RO.DevTest.Application.Features.Product.Queries.GetProductByIdQuery;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Domain.Entities;
using MediatR;
using RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;
using Microsoft.AspNetCore.Http.Extensions;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;
using RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

[Route("api/products")]
[OpenApiTag("Products")]
public class ProductController(IMediator mediator) : ControllerBase 
{
  private readonly IMediator _mediator = mediator;

  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Product>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    PaginatedResult<Product> products = await _mediator.Send(new GetAllProductsQuery(pageNumber, pageSize)); 
    return Ok(products);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetProduct(string id)
  {
    Product product = await _mediator.Send(new GetProductByIdQuery(id));
    return Ok(product);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand product)
  {
    Product createdProduct = await _mediator.Send(product);
    return Created(HttpContext.Request.GetDisplayUrl(), createdProduct);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
  {
    Product updatedProduct = await _mediator.Send(product);
    return Ok(updatedProduct);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    bool result = await _mediator.Send(new DeleteProductCommand(id));
    return result ? Ok() : NotFound();
  }
}
