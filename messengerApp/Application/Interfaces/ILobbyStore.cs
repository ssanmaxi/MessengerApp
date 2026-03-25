using messengerApp.Domain.Entities;

namespace messengerApp.Application.Interfaces;

public interface ILobbyStore
{
    public Task<Lobby> CreateLobby(int hostUserId);
    public Task<bool> JoinLobby(string lobbyCode, int userId);
    public Task<bool> LeaveLobby(string lobbyCode, int userId);
    public Task<Lobby?> GetLobby(string lobbyCode);
}