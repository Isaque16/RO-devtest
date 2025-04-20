namespace RO.DevTest.WebApi.Controllers;

using Application.Features;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.Product.Queries.GetAllProductsQuery;
using Application.Features.Product.Queries.GetProductByIdQuery;
using Domain.Entities;
using MediatR;
using Application.Features.Product.Commands.CreateProductCommand;
using Microsoft.AspNetCore.Http.Extensions;
using Application.Features.Product.Commands.UpdateProductCommand;
using Application.Features.Product.Commands.DeleteProductCommand;

[Route("api/products")]
[OpenApiTag("Products")]
public class ProductController(IMediator mediator) : ControllerBase 
{
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Product>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAllProducts([FromQuery] PaginationQuery pagination)
  {
    var products = await mediator.Send(new GetAllProductsQuery(pagination)); 
    return Ok(products);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetProduct(string id)
  {
    var product = await mediator.Send(new GetProductByIdQuery(id));
    return Ok(product);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand product)
  {
    var createdProduct = await mediator.Send(product);
    return Created(HttpContext.Request.GetDisplayUrl(), createdProduct);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
  {
    var updatedProduct = await mediator.Send(product);
    return Ok(updatedProduct);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    var result = await mediator.Send(new DeleteProductCommand(id));
    return result ? Ok() : NotFound();
  }
}
