using HRMS.API.DTOs.Auth;

namespace HRMS.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}