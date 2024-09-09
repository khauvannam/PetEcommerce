using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Features.Discounts;

namespace Product_API.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class DiscountController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return Ok(); // Or another appropriate response
        }
        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDiscount([FromBody] UpdateDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return Ok(); // Or another appropriate response
        }
        return BadRequest(result.ErrorTypes);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDiscount([FromBody] DeleteDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return Ok(); // Or another appropriate response
        }
        return BadRequest(result.ErrorTypes);
    }
}
