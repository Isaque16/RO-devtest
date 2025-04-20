namespace RO.DevTest.WebApi.Controllers;

using MediatR;
using Domain.Entities;
using NSwag.Annotations;
using Application.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using Application.Features.Sale.Commands.DeleteSaleCommand;
using Application.Features.Sale.Commands.UpdateSaleCommand;
using Application.Features.Sale.Queries.GetAllSalesQuery;
using Application.Features.Sale.Queries.GetSaleByIdQuery;
using Application.Features.Sale.Queries.GetAllSalesByPeriodQuery;
using Application.Features.Sale.Commands.CreateSaleCommand;

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
  public async Task<IActionResult> GetAllSales([FromQuery] PaginationQuery pagination)
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
  /// Obtém todas as vendas em um período específico com paginação.
  /// </summary>
  /// <param name="dateTimeRange">Intervalo de datas para filtrar as vendas.</param>
  /// <param name="pagination">Parâmetros de paginação.</param>
  /// <returns>Lista paginada das vendas no período especificado.</returns>
  [HttpGet("period")]
  [ProducesResponseType(typeof(GetAllSalesByPeriodResult), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAllSalesByPeriod(
    [FromBody] DateTimeRange dateTimeRange, [FromQuery] PaginationQuery pagination)
  {
    try
    {
      var sales = await mediator.Send(new GetAllSalesByPeriodQuery(dateTimeRange, pagination));
      return Ok(sales);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao buscar vendas.", Details = ex.Message });
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
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao buscar a venda.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Cria uma nova venda.
  /// </summary>
  /// <param name="sale">Comando contendo os detalhes da venda a ser criada.</param>
  /// <returns>A venda criada.</returns>
  [HttpPost]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand sale)
  {
    try
    {
      var createdSale = await mediator.Send(sale);
      return Created(HttpContext.Request.GetDisplayUrl(), createdSale);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao criar a venda.", Details = ex.Message });
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
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao atualizar a venda.", Details = ex.Message });
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
      return StatusCode(StatusCodes.Status500InternalServerError, 
        new { Message = "Erro ao deletar a venda.", Details = ex.Message });
    }
  }
}
