using Basket_API.Domains.Baskets;
using Basket_API.Features.Baskets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController(ISender sender) : ControllerBase
{
    [HttpPost("/api/[controller]/add/{basketId}")]
    public async Task<IActionResult> Update([FromBody] AddToBasketRequest request, string basketId)
    {
        var command = new AddToBasket.Command(
            request.CustomerId,
            basketId,
            request.BasketItemRequests
        );

        var result = await sender.Send(command);
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await sender.Send(new DeleteBasket.Command(id));
        if (!result.IsFailure)
        {
            return NoContent();
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await sender.Send(new GetBasketById.Query(id));
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return NotFound(result.ErrorTypes);
    }
}
