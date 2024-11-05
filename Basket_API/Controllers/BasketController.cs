using Basket_API.DTOs.Baskets;
using Basket_API.Features.Baskets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateBasketRequest request)
    {
        var command = new UpdateBasket.Command(request.CustomerId, request.BasketItemRequests);

        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTo([FromBody] AddToBasketRequest request)
    {
        var command = new AddToBasket.Command(request.CustomerId, request.UpdateBasketItemRequest);

        var result = await sender.Send(command);

        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{basketId:int}")]
    public async Task<IActionResult> Delete(int basketId)
    {
        var result = await sender.Send(new DeleteBasket.Command(basketId));
        if (!result.IsFailure)
            return NoContent();

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{basketId:int}")]
    public async Task<IActionResult> GetById(int basketId)
    {
        var result = await sender.Send(new GetBasketById.Query(basketId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
