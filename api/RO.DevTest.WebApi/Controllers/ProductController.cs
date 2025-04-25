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
[OpenApiTag("Products", Description = "Endpoints to manage products.")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
  /// <summary>
  /// Retrieves all products with pagination.
  /// </summary>
  /// <param name="pagination">Pagination parameters.</param>
  /// <returns>A paginated list of products.</returns>
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Product>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery pagination)
  {
    try
    {
      var products = await mediator.Send(pagination);
      return Ok(products);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error retrieving products.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Retrieves a product by its ID.
  /// </summary>
  /// <param name="id">The ID of the product.</param>
  /// <returns>The product corresponding to the given ID.</returns>
  [HttpGet("{id:guid}")]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetProductById(Guid id)
  {
    try
    {
      var query = new GetProductByIdQuery(id);
      var product = await mediator.Send(query);
      return Ok(product);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error retrieving the product.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Creates a new product.
  /// </summary>
  /// <param name="newProduct">Command containing the details of the product to be created.</param>
  /// <returns>The created product.</returns>
  [HttpPost]
  [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand newProduct)
  {
    try
    {
      var createdProduct = await mediator.Send(newProduct);
      return Created(HttpContext.Request.GetDisplayUrl(), createdProduct);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error creating the product.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Updates an existing product.
  /// </summary>
  /// <param name="product">Data of the product to be updated.</param>
  /// <returns>The updated product.</returns>
  [HttpPut]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
  {
    try
    {
      var updatedProduct = await mediator.Send(product);
      return Ok(updatedProduct);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error updating the product.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Deletes a product by its ID.
  /// </summary>
  /// <param name="id">The ID of the product to be deleted.</param>
  /// <returns>Confirmation of the deletion.</returns>
  [HttpDelete("{id:guid}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    try
    {
      var command = new DeleteProductCommand(id);
      var result = await mediator.Send(command);
      return result ? Ok() : NotFound(new { Message = "Product not found." });
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error deleting the product.", Details = ex.Message });
    }
  }
}
