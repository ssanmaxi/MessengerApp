using System.Security.Cryptography;
using messengerApp.Application.Interfaces;
using OtpNet;

namespace messengerApp.Infrastructure.Services;

public class TotpService : ITotpService
{
    public string GenerateSecret()
    {
        byte[] bytes = new byte[20];
        RandomNumberGenerator.Fill(bytes);

        var secret = Base32Encoding.ToString(bytes);
        return secret;
    }

    public bool VerifyCode(string secret, string code)
    {
        var secretBytes = Base32Encoding.ToBytes(secret);
        var totp = new Totp(secretBytes);

        var window = new VerificationWindow(previous: 1, future: 1);
        
        return totp.VerifyTotp(code, out _, window);
    }
}