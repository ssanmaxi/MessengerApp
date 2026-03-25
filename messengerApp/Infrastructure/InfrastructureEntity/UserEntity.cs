namespace messengerApp.Infrastructure.InfrastructureEntity;

public class UserEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public ProfileEntity Profile { get; set; } = null!;
    public bool TwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public string? Provider { get; set; }
    public string? ProviderUserId { get; set; }
}