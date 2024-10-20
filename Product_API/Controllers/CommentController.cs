using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.DTOs.Comments;
using Product_API.Features.Comments;

namespace Product_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateComment.Command command)
    {
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> Delete(int commentId)
    {
        var command = new DeleteComment.Command(commentId);
        if (await sender.Send(command) is { IsFailure: true } result)
            return BadRequest(result.ErrorTypes);

        return Ok();
    }

    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> Update(int commentId, [FromBody] UpdateCommentRequest request)
    {
        var command = new UpdateComment.Command(commentId, request);
        var result = await sender.Send(command);
        if (!result.IsFailure)
            return Ok();

        return NotFound(result.ErrorTypes);
    }

    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetById(int commentId)
    {
        var result = await sender.Send(new GetCommentById.Query(commentId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int limit,
        [FromQuery] int offset,
        [FromQuery] int? productId = default
    )
    {
        var result = await sender.Send(new GetAllComment.Query(limit, offset, productId));
        if (!result.IsFailure)
            return Ok(result.Value);

        return NotFound(result.ErrorTypes);
    }
}
