using messengerApp.Domain.Entities;
namespace messengerApp.Application.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}