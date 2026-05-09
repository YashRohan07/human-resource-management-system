using HRMS.API.Common;
using HRMS.API.DTOs.Auth;
using HRMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Authorized user access successful",
            Data = new
            {
                User = User.Identity?.Name
            }
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Admin access successful"
        });
    }
}