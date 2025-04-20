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
[OpenApiTags("Sales")]
public class SaleController(IMediator mediator) : ControllerBase
{
  [HttpGet]
  [ProducesResponseType(typeof(PaginatedResult<Sale>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetSales([FromQuery] PaginationQuery pagination)
  {
    var sales = await mediator.Send(new GetAllSalesQuery(pagination));
    return Ok(sales);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetSaleById(string id)
  {
    var sale = await mediator.Send(new GetSaleByIdQuery(id));
    return Ok(sale);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
  public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleCommand sale)
  {
    var updatedSale = await mediator.Send(sale);
    return Ok(updatedSale);
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
  public async Task<IActionResult> DeleteSale(string id)
  {
    var result = await mediator.Send(new DeleteSaleCommand(id));
    return result ? NoContent() : NotFound();
  }
}
