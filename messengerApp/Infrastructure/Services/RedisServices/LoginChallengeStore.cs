using messengerApp.Application.Interfaces;
using StackExchange.Redis;

namespace messengerApp.Services;

public class LoginChallengeStore : ILoginChallengeStore
{
    private readonly IConnectionMultiplexer _mux;

    public LoginChallengeStore(IConnectionMultiplexer mux)
    {
        _mux = mux;
    }
    
    private static string Key(string challengeId) => $"login_challenge:{challengeId}";
    
    public async Task SetAsync(string challengeId, int userId, TimeSpan ttl)
    {
        var db = _mux.GetDatabase();
        await db.StringSetAsync(Key(challengeId), userId, ttl);
    }

    public async Task<int?> GetAsync(string challengeId)
    {
        var db = _mux.GetDatabase();
        var value = await db.StringGetAsync(Key(challengeId));

        if (value.IsNullOrEmpty) return null;

        return int.TryParse(value.ToString(), out var userId) ? userId : null;
    }

    public async Task DeleteAsync(string challengeId)
    {
        var db = _mux.GetDatabase();
        await db.KeyDeleteAsync(Key(challengeId));
    }
}