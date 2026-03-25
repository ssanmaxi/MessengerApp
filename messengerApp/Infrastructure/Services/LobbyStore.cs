using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
using StackExchange.Redis;

namespace messengerApp.Infrastructure.Services;

public class LobbyStore : ILobbyStore
{
    private readonly IConnectionMultiplexer _mux;
    private readonly GenerateStrCode _gs;

    public LobbyStore(IConnectionMultiplexer mux, GenerateStrCode gs)
    {
        _mux = mux;
        _gs = gs;
    }

    private static string LobbyInfo(string lobbyCode) => $"lobby:{lobbyCode}";
    private static string Members(string lobbyCode) => $"lobby:{lobbyCode}:members";   

    public async Task<Lobby> CreateLobby(int hostUserId)
    {
        var db = _mux.GetDatabase();
        var code = _gs.GenerateLobbyCode();
        var infoKey = LobbyInfo(code);
        var membersKey = Members(code);
        
        await db.HashSetAsync(infoKey, "hostUserId", hostUserId);
        await db.SetAddAsync(membersKey, hostUserId); 
        return new Lobby 
        {
            Code = code, 
            HostUserId = hostUserId, 
            Members = [hostUserId]
        };
    }

    public async Task<bool> JoinLobby(string lobbyCode, int userId)
    {
        var db = _mux.GetDatabase();
        var infoKey = LobbyInfo(lobbyCode);
        var membersKey = Members(lobbyCode);

        if (!await db.KeyExistsAsync(infoKey))
        {
            return false;
        }

        await db.SetAddAsync(membersKey, userId);
        return true;
    }

    public async Task<bool> LeaveLobby(string lobbyCode, int userId)
    {
        var db = _mux.GetDatabase();
        var code = LobbyInfo(lobbyCode);
        var key = Members(lobbyCode);

        if (!await db.KeyExistsAsync(code))
        {
            return false;
        }

        await db.SetRemoveAsync(key, userId);
        if (await db.SetLengthAsync(key) == 0)
        {
            await db.KeyDeleteAsync(code);
            await db.KeyDeleteAsync(key);
            return true;
        }

        return true;
    }

    public async Task<Lobby?> GetLobby(string lobbyCode)
    {
        var db = _mux.GetDatabase();
        var infoKey = LobbyInfo(lobbyCode);
        var membersKey = Members(lobbyCode);

        if (!await db.KeyExistsAsync(infoKey))
        {
            return null;
        }

        var user = await db.HashGetAsync(infoKey, "hostUserId");
        var members = await db.SetMembersAsync(membersKey);
        var res = members.Select(x => (int)x).ToList();

        return new Lobby
        {
            Code = lobbyCode.Trim(),
            HostUserId = (int)user,
            Members = res
        };
    }
}