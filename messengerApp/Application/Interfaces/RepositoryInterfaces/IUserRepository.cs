using messengerApp.Domain.Entities;
namespace messengerApp.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIDAsync(int id);
    Task AddAsync(User user);
    Task SaveChangesAsync();
    Task SetAvatarUrlAsync(int userId, string avatarUrl);
    Task SetSecretAndState(string secret, bool state, int userId);
    Task<string> GetTFSecret(int userId);
    Task<User?> GetByGithubUserIdAsync(string id);
}