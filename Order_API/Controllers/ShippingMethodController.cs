using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Features.ShippingMethods;

namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingMethodsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShippingMethod(
        [FromBody] CreateShippingMethod.Command command
    )
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return CreatedAtAction(
                nameof(GetShippingMethodById),
                new { shippingMethodId = result.Value!.ShippingMethodId },
                result.Value
            );
        return BadRequest(result.ErrorTypes);
    }

    [HttpPut("{shippingMethodId}")]
    public async Task<IActionResult> UpdateShippingMethod(
        string shippingMethodId,
        [FromBody] UpdateShippingMethod.Command command
    )
    {
        if (shippingMethodId != command.ShippingMethodId)
            return BadRequest("Shipping Method ID mismatch.");

        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);
        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{shippingMethodId}")]
    public async Task<IActionResult> DeleteShippingMethod(string shippingMethodId)
    {
        var command = new DeleteShippingMethod.Command(shippingMethodId);
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return NoContent();
        return NotFound(result.ErrorTypes);
    }

    [HttpGet("{shippingMethodId}")]
    public async Task<IActionResult> GetShippingMethodById(string shippingMethodId)
    {
        var query = new GetShippingMethodById.Query(shippingMethodId);
        var result = await sender.Send(query);
        if (!result.IsFailure)
            return Ok(result.Value);
        return NotFound(result.ErrorTypes);
    }
}
