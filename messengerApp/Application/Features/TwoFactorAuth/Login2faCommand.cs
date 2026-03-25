namespace messengerApp.Application.Features.TwoFactorAuth;

public record Login2faCommand(string challengeId, string code);