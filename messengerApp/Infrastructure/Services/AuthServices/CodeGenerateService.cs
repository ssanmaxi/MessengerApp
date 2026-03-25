using messengerApp.Application.Auth.IssueTokenForOtp;
using messengerApp.Application.Interfaces;

namespace messengerApp.Infrastructure.Services;

public class CodeGenerateService
{
    private readonly IOtpStore _os;
    private readonly IssueTokenForOtpHandler _th;

    public CodeGenerateService(IOtpStore os, IssueTokenForOtpHandler th)
    {
        _os = os;
        _th = th;
    }

    public async Task<string> code(string email)
    {
        var a = new Random().Next(1000, 9999).ToString();
        await _os.SetAsync(email, a, TimeSpan.FromSeconds(60));

        return a;
    }

    public async Task<string> AlikhanSEX(string email, string code)
    {
        email = email.Trim();
        code = code.Trim();

        var saved = await _os.GetAsync(email);
        if (saved == null) return ("1");
        if (saved != code) return ("2");

        await _os.DeleteAsync(email);
        var token = await _th.Handle(new IssueTokenForOtpCommand(email));

        return token;
    }
}