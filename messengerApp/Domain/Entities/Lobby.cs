namespace messengerApp.Domain.Entities;

public class Lobby
{
    public required string Code { get; set; }
    public int HostUserId { get; set; }
    public required List<int> Members { get; set; }
}