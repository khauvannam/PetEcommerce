using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Products;
using Product_API.Features.Products;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpPost("/api/[controller]/add/")]
    public async Task<IActionResult> Add([FromForm] CreateProduct.Command command, string productId)
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete(string productId)
    {
        var result = await sender.Send(new DeleteProduct.Command(productId));
        if (!result.IsFailure)
            return NoContent();

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetById(string productId)
    {
        var result = await sender.Send(new GetProductById.Query(productId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var result = await sender.Send(new GetAllProducts.Query());
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> Update(
        string productId,
        [FromForm] UpdateProductRequest request
    )
    {
        var command = new UpdateProduct.Command(productId, request);
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }
}
