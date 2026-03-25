using messengerApp.Domain.Entities;

namespace messengerApp.Application.Interfaces;

public interface IInviteRepository
{
    public Task<Invite?> GetByTokenAsync(string token);
    Task DeleteByTokenAsync(string token);
    Task AddAsync(Invite invite);
}