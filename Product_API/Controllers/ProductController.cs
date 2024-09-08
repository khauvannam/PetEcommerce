using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Features.Products;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpPost("/api/[controller]/add/{productId}")]
    public async Task<IActionResult> Add([FromBody] CreateProduct.Command command, string productId)
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete(string productId)
    {
        var result = await sender.Send(new DeleteProduct.Command(productId));
        if (!result.IsFailure)
        {
            return NoContent();
        }

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await sender.Send(new GetProductById.Query(id));
        if (!result.IsFailure)
        {
            return Ok(result.Value);
        }

        return NotFound(result.ErrorTypes);
    }
}
