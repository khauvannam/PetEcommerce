using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Features.Discounts;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDiscount([FromBody] UpdateDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDiscount([FromBody] DeleteDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts()
    {
        var query = new GetAllDiscount.Query();
        var result = await sender.Send(query);
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }
}
