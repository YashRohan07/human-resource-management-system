using HRMS.API.Common;
using HRMS.API.DTOs.Auth;
using HRMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        // Controller should stay thin
        var result = await _authService.LoginAsync(request);

        var response = new ApiResponse<LoginResponseDto>
        {
            Success = true,
            Message = "Login successful",
            Data = result
        };

        return Ok(response);
    }
}