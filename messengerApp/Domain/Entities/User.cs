namespace messengerApp.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public string? PasswordHash { get; private set; }
    public Profile? Profile { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public string? TwoFactorSecret { get; private set; }
    public string? Provider { get; private set; }
    public string? ProviderUserId { get; private set; }
    
    public User() {}

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static User CreateFromOauth(string name, string email, string provider, string providerUserid)
    {
        var user = new User(name, email, passwordHash : null!);
        user.Provider = provider;
        user.ProviderUserId = providerUserid;
        return user;
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new Exception("Name cannot be empty!");
        }

        Name = newName;
    }

    public void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("Email cannot be empty!");

        Email = email;
    }

    public void EnableTwoFactor(string secret)
    {
        TwoFactorEnabled = true;
        TwoFactorSecret = secret;
    }

    public void DisableTwoFactor()
    {
        TwoFactorEnabled = false;
        TwoFactorSecret = null;
    }

    public static User Reconstitute(
        int id, string name, string email, string passwordHash,
        string? provider, string? providerUserId, Profile? profile,
        bool twoFactorEnabled, string? twoFactorSecret)
    {
        var user = new User();
        user.Id = id;
        user.Name = name;
        user.Email = email;
        user.PasswordHash = passwordHash;
        user.Provider = provider;
        user.ProviderUserId = providerUserId;
        user.Profile = profile;
        user.TwoFactorEnabled = twoFactorEnabled;
        user.TwoFactorSecret = twoFactorSecret;

        return user;
    }
}