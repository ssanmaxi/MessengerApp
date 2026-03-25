namespace messengerApp.Application.Interfaces;

public interface ILoginChallengeStore
{
    Task SetAsync(string challengeId, int userId, TimeSpan ttl);
    Task<int?> GetAsync(string challengeId);
    Task DeleteAsync(string challengeId);
}