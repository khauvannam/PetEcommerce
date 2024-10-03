using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Discounts;
using Product_API.DTOs.Discounts;
using Product_API.Features.Discounts;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromForm] CreateDiscount.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{discountId}")]
    public async Task<IActionResult> DeleteDiscount(Guid discountId)
    {
        var result = await sender.Send(new DeleteDiscount.Command(discountId));
        if (result.IsFailure)
            return Ok(); // Or another appropriate response
        return BadRequest(result.ErrorTypes);
    }

    [HttpPut("{discountId}")]
    public async Task<IActionResult> UpdateDiscount(
        Guid discountId,
        [FromForm] DiscountRequest request
    )
    {
        var result = await sender.Send(new UpdateDiscount.Command(discountId, request));
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
