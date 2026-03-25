namespace messengerApp.Infrastructure.InfrastructureEntity;

public class ProfileEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    public string? AvatarUrl { get; set; }
}