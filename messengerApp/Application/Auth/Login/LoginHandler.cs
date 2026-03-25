using messengerApp.Application.Interfaces;
namespace messengerApp.Application.Auth.Login;

public class LoginHandler
{
    private readonly IUserRepository _ur;
    private readonly IPasswordHasher _ph;
    private readonly ITokenService _ts;

    public LoginHandler(IUserRepository ur, IPasswordHasher ph, ITokenService ts)
    {
        _ur = ur;
        _ph = ph;
        _ts = ts;
    }

    public async Task<string> Handle(LoginCommand command)
    {
        var user = await _ur.GetByEmailAsync(command.email);

        if (user == null)
        {
            throw new Exception($"No user with email {command.email} was found!");
        }

        var check = _ph.Verify(command.password, user.PasswordHash);
        if (!check)
        {
            throw new Exception("Passwords do not match");
        }

        var token = _ts.GenerateToken(user);
        return token;
    }
}