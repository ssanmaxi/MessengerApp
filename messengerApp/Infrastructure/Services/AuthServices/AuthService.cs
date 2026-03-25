using messengerApp.Application.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using BCrypt.Net;
using messengerApp.Application.Interfaces.Services;
using messengerApp.Infrastructure.InfrastructureEntity;
namespace messengerApp.Application.Services;

public class AuthService : IAuthService
{
    public async Task<UserResponseDTO> RegisterAsync(RegisterDTO reg)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(reg.Password);
        
        var user = new UserEntity
        {
            Id = 1, 
            Name = reg.Name,
            Email = reg.Email
        };

        string token = GenerateJwtToken(user);

        return new UserResponseDTO
        {
            Name = user.Name,
            Email = user.Email,
            Token = token
        };
    }
    
    public async Task<string> LoginAsync(LoginDTO log)
    {
        if (log.Email == "test@test.com" && log.Password == "admin123")
        {
            var user = new UserEntity
            {
                Id = 1,
                Name = "TestAdmin",
                Email = log.Email
            };
            return GenerateJwtToken(user);
        }

        throw new Exception("Неверный логин или пароль! (Попробуй test@test.com / admin123)");
    }

    public Task<string> IssueTokenForOtpAsync(string email)
    {
        var user = new UserEntity
        {
            Id = 1,
            Name = email.Split('@')[0],
            Email = email
        };

        var token = GenerateJwtToken(user);
        return Task.FromResult(token);
    }
    
    private string GenerateJwtToken(UserEntity userEntity)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
            new Claim(ClaimTypes.Email, userEntity.Email),
            new Claim(ClaimTypes.Name, userEntity.Name)
        };
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("SuperSecretKey1234567890!_LONG_ENOUGH_KEY_32_CHARS"));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "my-auth-server",
            audience: "my-api",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}