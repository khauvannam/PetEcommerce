using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Features.Categories;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ISender sender) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] CreateCategory.Command command)
    {
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(string categoryId)
    {
        var command = new DeleteCategory.Command(categoryId);
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetById(string categoryId)
    {
        var result = await sender.Send(new GetCategoryById.Query(categoryId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var result = await sender.Send(new GetAllCategories.Query());
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
