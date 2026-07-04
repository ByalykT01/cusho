using cusho.Dtos.UserDtos;
using cusho.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[Route("api/[controller]")]
public class AuthController(AuthService authService) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<Results<
            CreatedAtRoute<UserResponseDto>,
            ProblemHttpResult
        >> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var result = await authService.RegisterUserAsync(userRegistrationDto);

        if (result.IsFailure)
        {
            return BadRequestProblem(result.Error);
        }

        return TypedResults.CreatedAtRoute(result.Value, nameof(UsersController.GetUserById), new { userId = result.Value.Id });
    }

    [HttpPost("login")]
    public async Task<Results<
            Ok<LoginResponseDto>,
            ProblemHttpResult
        >> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);

        if (result.IsFailure)
        {
            return UnauthorizedProblem();
        }

        return TypedResults.Ok(result.Value);
    }
}
