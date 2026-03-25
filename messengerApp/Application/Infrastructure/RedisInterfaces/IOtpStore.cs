using StackExchange.Redis;
namespace messengerApp.Application.Interfaces;

public interface IOtpStore
{
    Task SetAsync(string email, string code, TimeSpan ttl);
    Task<string?> GetAsync(string email);
    Task DeleteAsync(string email);
}