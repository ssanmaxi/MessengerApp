namespace messengerApp.Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string? AvatarUrl { get; set; }
}