using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
using messengerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using messengerApp.Infrastructure.InfrastructureEntity;

namespace messengerApp.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await _db.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(f => f.Email == email);

        return entity != null ? ToDomain(entity) : null;
    }

    public async Task<User?> GetByIDAsync(int id)
    {
        var entity = await _db.Users
            .Include(e=>e.Profile)
            .FirstOrDefaultAsync(e => e.Id == id);

        return entity != null ? ToDomain(entity) : null;
    }

    public async Task AddAsync(User user)
    {
        _db.Users.Add(ToEntity(user));
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task SetAvatarUrlAsync(int userId, string avatarUrl)
    {
        var entity = await _db.Users
            .Include(p => p.Profile)
            .FirstOrDefaultAsync(p => p.Id == userId);

        if (entity == null || entity.Profile == null)
        {
            throw new Exception("Profile not found");
        }

        entity.Profile.AvatarUrl = avatarUrl;
        await _db.SaveChangesAsync();
    }

    public async Task SetSecretAndState(string secret, bool state, int userId)
    {
        var entity = await _db.Users
            .FirstOrDefaultAsync(p => p.Id == userId);

        if (entity == null)
        {
            throw new Exception("Alikhan spank u");
        }

        entity.TwoFactorSecret = secret;
        entity.TwoFactorEnabled = state;

        await _db.SaveChangesAsync();
    }

    public async Task<string> GetTFSecret(int userId)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(d => d.Id == userId);
        return user.TwoFactorSecret;
    }

    public async Task<User?> GetByGithubUserIdAsync(string id)
    {
        var user = await _db.Users
            .Include(p=>p.Profile)
            .FirstOrDefaultAsync(d => d.ProviderUserId == id);

        return user != null ? ToDomain(user) : null;
    }

    private static User ToDomain(UserEntity e) => User.Reconstitute(
        e.Id, e.Name, e.Email, e.PasswordHash, e.Provider, e.ProviderUserId,
        e.Profile == null ? null : new Profile{},
        e.TwoFactorEnabled, e.TwoFactorSecret
    );

    private static UserEntity ToEntity(User u) => new UserEntity
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        PasswordHash = u.PasswordHash,
        Provider = u.Provider,
        ProviderUserId = u.ProviderUserId,
        Profile = u.Profile == null ? null! : new ProfileEntity
        {
        Name = u.Profile.Name,
        Age = u.Profile.Age, 
        AvatarUrl = u.Profile.AvatarUrl
        }
    };
}