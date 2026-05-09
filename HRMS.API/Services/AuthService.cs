using HRMS.API.Data;
using HRMS.API.DTOs.Auth;
using HRMS.API.Exceptions;
using HRMS.API.Security;
using HRMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly PasswordHasher _passwordHasher;

    public AuthService(
        ApplicationDbContext context,
        JwtHelper jwtHelper,
        PasswordHasher passwordHasher)
    {
        _context = context;
        _jwtHelper = jwtHelper;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Find user by login email
        var user = await _context.AppUsers
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        // Compare given password with stored password hash
        var isPasswordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        // Create JWT token after successful login
        var tokenResult = _jwtHelper.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = tokenResult.Token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            ExpiresAt = tokenResult.ExpiresAt
        };
    }
}