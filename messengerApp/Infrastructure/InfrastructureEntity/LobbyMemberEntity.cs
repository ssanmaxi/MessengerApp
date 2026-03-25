namespace messengerApp.Infrastructure.InfrastructureEntity;

public class LobbyMemberEntity
{
    public int Id { get; set; }
    public required string LobbyId { get; set; }
    public int UserId { get; set; }
}