using Basket_API.Domains.Baskets;
using Basket_API.Features.Baskets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController(ISender sender) : ControllerBase
{
    [HttpPost("/api/[controller]/{basketId}")]
    public async Task<IActionResult> Update([FromBody] AddToBasketRequest request, Guid basketId)
    {
        var command = new AddToBasket.Command(
            basketId,
            request.CustomerId,
            request.BasketItemRequests
        );

        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{basketId}")]
    public async Task<IActionResult> Delete(Guid basketId)
    {
        var result = await sender.Send(new DeleteBasket.Command(basketId));
        if (!result.IsFailure)
            return NoContent();

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{basketId}")]
    public async Task<IActionResult> GetById(Guid basketId)
    {
        var result = await sender.Send(new GetBasketById.Query(basketId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
