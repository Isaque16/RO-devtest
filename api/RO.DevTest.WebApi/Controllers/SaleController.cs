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
[OpenApiTag("Sales", Description = "Endpoints to manage sales.")]
[ApiController]
public class SaleController(IMediator mediator) : ControllerBase
{
  /// <summary>
  /// Retrieves all sales with pagination.
  /// </summary>
  /// <param name="pagination">Pagination parameters.</param>
  /// <returns>A paginated list of sales.</returns>
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Sale>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllSales([FromQuery] GetAllSalesQuery pagination)
  {
    try
    {
      var sales = await mediator.Send(pagination);
      return Ok(sales);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error retrieving sales.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Retrieves all sales within a specific period with pagination.
  /// </summary>
  /// <param name="pagination">Pagination parameters.</param>
  /// <param name="dateRange">Date range to filter sales.</param>
  /// <returns>A paginated list of sales within the specified period.</returns>
  [HttpGet("period")]
  [ProducesResponseType(typeof(GetAllSalesByPeriodResult), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllSalesByPeriod(
    [FromQuery] PaginationQuery pagination,
    [FromBody] DateTimeRange dateRange)
  {
    try
    {
      var query = new GetAllSalesByPeriodQuery
      {
        Pagination = pagination,
        DateRange = dateRange
      };

      var sales = await mediator.Send(query);
      return Ok(sales);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error retrieving sales.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Retrieves a sale by its ID.
  /// </summary>
  /// <param name="id">The ID of the sale.</param>
  /// <returns>The sale corresponding to the given ID.</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetSaleById(GetSaleByIdQuery id)
  {
    try
    {
      var sale = await mediator.Send(id);
      return Ok(sale);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error retrieving the sale.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Creates a new sale.
  /// </summary>
  /// <param name="newSale">Command containing the details of the sale to be created.</param>
  /// <returns>The created sale.</returns>
  [HttpPost]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand newSale)
  {
    try
    {
      var createdSale = await mediator.Send(newSale);
      return Created(HttpContext.Request.GetDisplayUrl(), createdSale);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error creating the sale.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Updates an existing sale.
  /// </summary>
  /// <param name="sale">Data of the sale to be updated.</param>
  /// <returns>The updated sale.</returns>
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
        new { Message = "Error updating the sale.", Details = ex.Message });
    }
  }

  /// <summary>
  /// Deletes a sale by its ID.
  /// </summary>
  /// <param name="id">The ID of the sale to be deleted.</param>
  /// <returns>Confirmation of the deletion.</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteSale(DeleteSaleCommand id)
  {
    try
    {
      var result = await mediator.Send(id);
      return result ? NoContent() : NotFound(new { Message = "Sale not found." });
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new { Message = "Error deleting the sale.", Details = ex.Message });
    }
  }
}
