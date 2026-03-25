namespace messengerApp.Application.Interfaces;

public interface ILobbyMemberRepository
{
    public Task<bool> IsMemberAsync(string lobbyId, int userId);
    public Task AddAsync(string lobbyId, int userId);
}