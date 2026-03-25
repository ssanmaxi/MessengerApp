using StackExchange.Redis;
using messengerApp.Application.Interfaces;
namespace messengerApp.Services;

public class RedisOtpStore : IOtpStore
{
    private readonly IConnectionMultiplexer _mux;

    public RedisOtpStore(IConnectionMultiplexer mux)
    {
        _mux = mux;
    }

    private static string Key(string email) => $"otp:{email.Trim()}";

    public async Task SetAsync(string email, string code, TimeSpan ttl)
    {
        var db = _mux.GetDatabase();
        await db.StringSetAsync(Key(email), code, ttl);
    }

    public async Task<string?> GetAsync(string email)
    {
        var db = _mux.GetDatabase();
        var value = await db.StringGetAsync(Key(email));
        return value.IsNullOrEmpty ? null : value.ToString();
    }

    public async Task DeleteAsync(string email)
    {
        var db = _mux.GetDatabase();
        await db.KeyDeleteAsync(Key(email));
    }
}