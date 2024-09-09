using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Features.Orders;

namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrder.Command command)
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = result.Value.OrderId },
                result.Value
            );
        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        var command = new DeleteOrder.Command(orderId);
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return NoContent();
        return NotFound(result.ErrorTypes);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(string orderId)
    {
        var query = new GetOrderById.Query(orderId);
        var result = await sender.Send(query);
        if (!result.IsFailure)
            return Ok(result.Value);
        return NotFound(result.ErrorTypes);
    }
}
