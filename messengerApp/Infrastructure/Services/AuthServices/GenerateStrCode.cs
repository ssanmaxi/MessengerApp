namespace messengerApp.Infrastructure.Services;

public class GenerateStrCode
{
    public string GenerateLobbyCode()
    {
        const string pool = "abcdefghijklmnopqrstuvwxyz";
        var arr = Enumerable.Range(0, 5)
            .Select(_ => pool[Random.Shared.Next(pool.Length)])
            .ToArray();

        return new string(arr);
    }
}