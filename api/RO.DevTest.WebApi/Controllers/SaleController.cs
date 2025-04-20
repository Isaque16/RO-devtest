namespace RO.DevTest.WebApi.Controllers;

using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Application.Features.Sale.Commands.DeleteSaleCommand;
using Application.Features.Sale.Commands.UpdateSaleCommand;
using Application.Features.Sale.Queries.GetAllSalesQuery;
using Application.Features.Sale.Queries.GetSaleByIdQuery;
using Domain.Entities;

[Route("api/sales")]
[OpenApiTag("Sales", Description = "Endpoints para gerenciar vendas.")]
[ApiController]
public class SaleController(IMediator mediator) : ControllerBase
{
  /// <summary>
  /// Obtém todas as vendas com paginação.
  /// </summary>
  /// <param name="pagination">Parâmetros de paginação.</param>
  /// <returns>Lista paginada de vendas.</returns>
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Sale>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetSales([FromQuery] PaginationQuery pagination)
  {
    try
    {
      var sales = await mediator.Send(new GetAllSalesQuery(pagination));
      return Ok(sales);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao buscar vendas.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Obtém uma venda pelo ID.
  /// </summary>
  /// <param name="id">ID da venda.</param>
  /// <returns>A venda correspondente ao ID.</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetSaleById(string id)
  {
    try
    {
      var sale = await mediator.Send(new GetSaleByIdQuery(id));
      if (sale == null)
        return NotFound(new { Message = "Venda não encontrada." });

      return Ok(sale);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao buscar a venda.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Atualiza uma venda existente.
  /// </summary>
  /// <param name="sale">Dados da venda a ser atualizada.</param>
  /// <returns>A venda atualizada.</returns>
  [HttpPut]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleCommand sale)
  {
    try
    {
      var updatedSale = await mediator.Send(sale);
      return Ok(updatedSale);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao atualizar a venda.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Deleta uma venda pelo ID.
  /// </summary>
  /// <param name="id">ID da venda a ser deletada.</param>
  /// <returns>Confirmação da exclusão.</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteSale(string id)
  {
    try
    {
      var result = await mediator.Send(new DeleteSaleCommand(id));
      return result ? NoContent() : NotFound(new { Message = "Venda não encontrada." });
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao deletar a venda.", Details = ex.Message });
    }
  }
}
