using System.Security.Claims;
using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[Route("api/[controller]")]
public sealed class UsersController(UsersService usersService) : ApiControllerBase
{
    [Authorize]
    [HttpGet("{userId}", Name = nameof(GetUserById))]
    public async Task<Results<
        Ok<UserResponseDto>,
        ProblemHttpResult
    >> GetUserById(Guid userId)
    {
        var result = await usersService.GetUserByIdAsync(userId);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    [Authorize("IsAdmin")]
    [HttpGet]
    public async Task<Ok<List<UserResponseDto>>> GetUsers()
    {
        var result = await usersService.GetAllUsersAsync();

        return TypedResults.Ok(result.Value);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<Results<
        NoContent,
        ProblemHttpResult
    >> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return UnauthorizedProblem();
        }

        var result = await usersService.ChangePasswordAsync(userId, changePasswordDto);

        if (result.IsFailure)
        {
            return result.Error == "User not found"
                ? NotFoundProblem(result.Error)
                : BadRequestProblem(result.Error);
        }

        return TypedResults.NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<Results<
        Ok<UserResponseDto>,
        ProblemHttpResult
    >> GetLoggedInUser()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return UnauthorizedProblem();
        }

        var result = await usersService.GetUserByIdAsync(userId);

        if (result.IsFailure)
        {
            return NotFoundProblem(result.Error);
        }

        return TypedResults.Ok(result.Value);
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out userId);
    }

}
