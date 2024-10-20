using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.DTOs.Products;
using Product_API.Features.Products;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CreateProduct.Command command)
    {
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }

    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> Delete(int productId)
    {
        var result = await sender.Send(new DeleteProduct.Command(productId));
        if (!result.IsFailure)
            return NoContent();

        return BadRequest(result.ErrorTypes);
    }

    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetById(int productId)
    {
        var result = await sender.Send(new GetProductById.Query(productId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int limit,
        [FromQuery] int offset,
        [FromQuery] bool isBestSeller = false,
        [FromQuery] int? categoryId = default
    )
    {
        var result = await sender.Send(
            new GetAllProducts.Query(categoryId, limit, offset, isBestSeller)
        );
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Update(int productId, [FromForm] UpdateProductRequest request)
    {
        var command = new UpdateProduct.Command(productId, request);
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return BadRequest(result.ErrorTypes);
    }
}
