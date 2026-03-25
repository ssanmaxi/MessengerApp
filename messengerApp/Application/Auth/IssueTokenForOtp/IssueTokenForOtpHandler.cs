using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
namespace messengerApp.Application.Auth.IssueTokenForOtp;

public class IssueTokenForOtpHandler
{
    private readonly IUserRepository _ur;
    private readonly ITokenService _ts;

    public IssueTokenForOtpHandler(IUserRepository ur, ITokenService ts)
    {
        _ur = ur;
        _ts = ts;
    }

    public async Task<string> Handle(IssueTokenForOtpCommand command)
    {
        var user = await _ur.GetByEmailAsync(command.email);

        if (user == null)
        {
            throw new Exception("User not found!");
        }

        return _ts.GenerateToken(user);
    }
}