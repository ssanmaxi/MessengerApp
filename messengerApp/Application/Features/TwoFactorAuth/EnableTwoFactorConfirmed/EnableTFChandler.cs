using messengerApp.Application.Interfaces;
namespace messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorConfirmed;

public class EnableTFChandler
{
    private readonly ICurrentUser _cu;
    private readonly ITotpService _ts;
    private readonly IUserRepository _ur;
    private readonly ILogger<EnableTFChandler> _logger;

    public EnableTFChandler(ICurrentUser cu, ITotpService ts, IUserRepository ur, ILogger<EnableTFChandler> logger)
    {
        _cu = cu;
        _ts = ts;
        _ur = ur;
        _logger = logger;
    }

    public async Task Handle(EnableTFCcommand command)
    {
        _logger.LogInformation("Enabling Two Factor Confirmation");
        var userId = _cu.UserId;
        //var user = await _ur.GetByIDAsync(userId);
        var secret = await _ur.GetTFSecret(userId);

        if (secret == null) throw new Exception($"User with ID: {userId} was not found!");
        //Console.WriteLine(user.TwoFactorSecret);
        if (secret == null) throw new Exception("SGL1");

        
        if (string.IsNullOrWhiteSpace(command.code)) throw new Exception("CODE EMPTY");
        var result = _ts.VerifyCode(secret, command.code);

        if (!result) throw new Exception("SGL2");
    }
}