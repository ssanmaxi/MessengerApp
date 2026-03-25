namespace messengerApp.Application.Interfaces;

public interface ITotpService
{
    public string GenerateSecret();
    public bool VerifyCode(string secret, string code);
}