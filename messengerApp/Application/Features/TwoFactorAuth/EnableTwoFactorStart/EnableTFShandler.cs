using messengerApp.Application.Interfaces;
namespace messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorStart;

public class EnableTFShandler
{
    private readonly ICurrentUser _cu;
    private readonly IUserRepository _ur;
    private readonly ITotpService _ts;
    private readonly IUnitOfWork _uow;

    public EnableTFShandler(ICurrentUser cu, IUserRepository ur, ITotpService ts, IUnitOfWork ouw)
    {
        _cu = cu;
        _ur = ur;
        _ts = ts;
        _uow = ouw;
    }

    public async Task<string> Handle()
    {
        var userId = _cu.UserId;
        var user = await _ur.GetByIDAsync(userId);

        if (user == null) throw new Exception("sglw pobr");

        if (user.TwoFactorEnabled == true) throw new Exception("sgl");

        var secret = _ts.GenerateSecret();
        await _ur.SetSecretAndState(secret, true, userId);
        
        return secret;
    }
}