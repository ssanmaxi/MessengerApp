using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
using messengerApp.Infrastructure.Data;
using messengerApp.Infrastructure.InfrastructureEntity;
using Microsoft.EntityFrameworkCore;

namespace messengerApp.Infrastructure.Repository;

public class InviteRepository : IInviteRepository
{
    private readonly AppDbContext _db;

    public InviteRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Invite?> GetByTokenAsync(string token)
    {
        var entity = await _db.Invites.FirstOrDefaultAsync(t => t.Token == token);

        if (entity is null) return null;

        return new Invite
        {
            Token = entity.Token,
            LobbyId = entity.LobbyId,
            ExpiresAt = entity.ExpiresAt,
            Revoked = entity.Revoked
        };
    }

    public async Task DeleteByTokenAsync(string token)
    {
        var entity = await _db.Invites.FirstOrDefaultAsync(t => t.Token == token);

        if (entity is null) return;

        _db.Invites.Remove(entity);
    }
    
    public Task AddAsync(Invite invite)
    {
        _db.Invites.Add(new InviteEntity
        {
            Token = invite.Token,
            LobbyId = invite.LobbyId,
            ExpiresAt = invite.ExpiresAt,
            Revoked = invite.Revoked
        });

        return Task.CompletedTask;
    }
}   