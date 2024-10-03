using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Categories;
using Product_API.DTOs.Categories;
using Product_API.Features.Categories;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategory.Command command)
    {
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpDelete("{categoryId:int}")]
    public async Task<IActionResult> Delete(int categoryId)
    {
        var command = new DeleteCategory.Command(categoryId);
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpPut("{categoryId:int}")]
    public async Task<IActionResult> Update(
        int categoryId,
        [FromBody] UpdateCategoryRequest request
    )
    {
        var command = new UpdateCategory.Command(
            categoryId,
            request.CategoryName,
            request.Description,
            request.File
        );
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet("{categoryId:int}")]
    public async Task<IActionResult> GetById(int categoryId)
    {
        var result = await sender.Send(new GetCategoryById.Query(categoryId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet("{categoryEndpoint}")]
    public async Task<IActionResult> GetByEndpoint(string categoryEndpoint)
    {
        var result = await sender.Send(new GetCategoryByEndpoint.Query(categoryEndpoint));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await sender.Send(new GetAllCategories.Query());
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
