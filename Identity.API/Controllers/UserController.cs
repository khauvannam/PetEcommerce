using Identity.API.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(Register.Command command)
    {
        var validationResult = await new Register.Validator().ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            // Handle validation errors
            return BadRequest(validationResult.Errors);
        }
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorType);
        }
        return Ok("User is created successfully");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(Login.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorType);
        }

        return Ok(result);
    }

    [HttpGet(nameof(ForgotPassword))]
    public async Task<IActionResult> ForgotPassword(ForgotPassword.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorType);
        }

        return Ok(result);
    }

    [HttpPost(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword(ResetPassword.Command command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.ErrorType);
        }
        return Ok(result);
    }
}
