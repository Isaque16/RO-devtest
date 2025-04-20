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
[OpenApiTag("Products", Description = "Endpoints para gerenciar produtos.")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
  /// <summary>
  /// Obtém todos os produtos com paginação.
  /// </summary>
  /// <param name="pagination">Parâmetros de paginação.</param>
  /// <returns>Lista paginada de produtos.</returns>
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Product>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllProducts([FromQuery] PaginationQuery pagination)
  {
    try
    {
      var products = await mediator.Send(new GetAllProductsQuery(pagination));
      return Ok(products);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao buscar produtos.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Obtém um produto pelo ID.
  /// </summary>
  /// <param name="id">ID do produto.</param>
  /// <returns>O produto correspondente ao ID.</returns>
  [HttpGet("{id:guid}")]
  [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetProduct(Guid id)
  {
    try
    {
      var product = await mediator.Send(new GetProductByIdQuery(id));
      if (product == null)
        return NotFound(new { Message = "Produto não encontrado." });
      return Ok(product);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao buscar o produto.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Cria um novo produto.
  /// </summary>
  /// <param name="product">Dados do produto a ser criado.</param>
  /// <returns>O produto criado.</returns>
  [HttpPost]
  [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand product)
  {
    try
    {
      var createdProduct = await mediator.Send(product);
      return Created(HttpContext.Request.GetDisplayUrl(), createdProduct);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao criar o produto.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Atualiza um produto existente.
  /// </summary>
  /// <param name="product">Dados do produto a ser atualizado.</param>
  /// <returns>O produto atualizado.</returns>
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
        new { Message = "Erro ao atualizar o produto.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Deleta um produto pelo ID.
  /// </summary>
  /// <param name="id">ID do produto a ser deletado.</param>
  /// <returns>Confirmação da exclusão.</returns>
  [HttpDelete("{id:guid}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    try
    {
      var result = await mediator.Send(new DeleteProductCommand(id));
      return result ? Ok() : NotFound(new { Message = "Produto não encontrado." });
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao deletar o produto.", Details = ex.Message });
    }
  }
}
