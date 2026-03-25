using messengerApp.Application.DTO;
namespace messengerApp.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDTO log);
    Task<UserResponseDTO> RegisterAsync(RegisterDTO reg);
    Task<string> IssueTokenForOtpAsync(string email);
}