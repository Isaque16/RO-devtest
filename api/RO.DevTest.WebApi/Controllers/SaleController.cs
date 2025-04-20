using RO.DevTest.Application.Features;

namespace RO.DevTest.WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Sale.Commands.DeleteSaleCommand;
using RO.DevTest.Application.Features.Sale.Commands.UpdateSaleCommand;
using RO.DevTest.Application.Features.Sale.Queries.GetAllSalesQuery;
using RO.DevTest.Application.Features.Sale.Queries.GetSaleByIdQuery;
using RO.DevTest.Application.Features.User.Queries;
using RO.DevTest.Domain.Entities;

[Route("api/sales")]
[OpenApiTags("Sales")]
public class SaleController(IMediator mediator) : ControllerBase
{
  private readonly IMediator _mediator = mediator;

  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Sale>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetSales([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    var sales = await _mediator.Send(new GetAllSalesQuery(pageNumber, pageSize));
    return Ok(sales);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetSaleById(string id)
  {
    var sale = await _mediator.Send(new GetSaleByIdQuery(id));
    return sale == null ? NotFound() : Ok(sale);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleCommand sale)
  {
    var updatedSale = await _mediator.Send(sale);
    return updatedSale == null ? NotFound() : Ok(updatedSale);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DeleteSale(string id)
  {
    var result = await _mediator.Send(new DeleteSaleCommand(id));
    return result ? NoContent() : NotFound();
  }
}
