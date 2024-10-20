using Basket_API.DTOs.Baskets;
using Basket_API.Features.Baskets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket_API.Controllers;

[ApiController]
[Route("api/[controller]/{basketId:int}")]
public class BasketController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] AddToBasketRequest request, int basketId)
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

    [HttpDelete]
    public async Task<IActionResult> Delete(int basketId)
    {
        var result = await sender.Send(new DeleteBasket.Command(basketId));
        if (!result.IsFailure)
            return NoContent();

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int basketId)
    {
        var result = await sender.Send(new GetBasketById.Query(basketId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
