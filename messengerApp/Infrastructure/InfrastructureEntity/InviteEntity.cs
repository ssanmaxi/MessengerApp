namespace messengerApp.Infrastructure.InfrastructureEntity;

public class InviteEntity
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public required string LobbyId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UsesCount { get; set; } 
    public int MaxUses { get; set; } 
    public bool Revoked { get; set; }
}