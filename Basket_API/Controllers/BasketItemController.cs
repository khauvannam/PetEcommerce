using Basket_API.Features.BasketItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket_API.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketItemController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBasketItem.Command command)
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketItem.Command command)
    {
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
        var result = await sender.Send(new DeleteBasketItem.Command(id));
        if (!result.IsFailure)
        {
            return NoContent();
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await sender.Send(new GetBasketItemById.Query(id));
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return NotFound(result.ErrorTypes);
    }
}
