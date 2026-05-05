using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Commands;
using OrderManagement.Application.Exceptions;
using OrderManagement.Application.Queries;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetAll(CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), ct);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetById(int id, CancellationToken ct)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);
            return Ok(order);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken ct)
    {
        try
        {
            var id = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (CustomerNotFoundException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateOrderCommand body,
        CancellationToken ct)
    {
        if (id != body.OrderId)
        {
            return BadRequest(new { message = "Route id and body OrderId mismatch." });
        }

        try
        {
            await _mediator.Send(body, ct);
            return NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new GetOrderByIdQuery(id), ct);
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }

        // Por simplicidad pedagógica delegamos en el repositorio directamente.
        var repo = HttpContext.RequestServices
            .GetRequiredService<OrderManagement.Application.Abstractions.IOrderRepository>();
        await repo.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new CancelOrderCommand(id), ct);
            return NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOrderStateException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }
}
