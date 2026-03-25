using messengerApp.Application.Interfaces;
using messengerApp.Infrastructure.Data;
using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;

namespace messengerApp.Infrastructure.Repository;

public class LobbyMemberRepository : ILobbyMemberRepository
{
    private readonly AppDbContext _db;

    public LobbyMemberRepository(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<bool> IsMemberAsync(string lobbyId, int userId)
    {
        return await _db.LobbyMembers
            .AnyAsync(x => x.LobbyId == lobbyId && x.UserId == userId);
    }

    public async Task AddAsync(string lobbyId, int userId)
    {
        var exists = await _db.LobbyMembers
            .AnyAsync(x => x.LobbyId == lobbyId && x.UserId == userId);

        if (exists) return;

        _db.LobbyMembers.Add(new LobbyMemberEntity
        {
            LobbyId = lobbyId,
            UserId = userId
        });
    }
}