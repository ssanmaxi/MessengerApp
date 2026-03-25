using Microsoft.Extensions.DependencyInjection;
using messengerApp.Application.Auth.IssueTokenForOtp;
using messengerApp.Application.Auth.Login;
using messengerApp.Application.Auth.Register;
using messengerApp.Application.Features.Invite.AcceptInvite;
using messengerApp.Application.Features.Invite.CreateInvite;
using messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorConfirmed;
using messengerApp.Application.Features.TwoFactorAuth.EnableTwoFactorStart;
using messengerApp.Application.Interfaces;
using messengerApp.Application.Profile.GetProfileByUserId;
using messengerApp.Application.Profile.UploadAvatar;
using messengerApp.Application.SaveMessage.Handler;
using messengerApp.Infrastructure.Repository;
using messengerApp.Infrastructure.Services;
using messengerApp.Services;


namespace messengerApp.Application.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<IssueTokenForOtpHandler>();
        services.AddScoped<SaveMessageHandler>();
        services.AddScoped<GetProfileByUserIdHandler>();
        services.AddScoped<UploadAvatarHandler>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IInviteRepository, InviteRepository>();
        services.AddScoped<ILobbyMemberRepository, LobbyMemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<AcceptInviteHandler>();
        services.AddScoped<CreateInviteHandler>();
        services.AddScoped<ITotpService, TotpService>();
        services.AddScoped<ILoginChallengeStore, LoginChallengeStore>();
        services.AddScoped<EnableTFChandler>();
        services.AddScoped<EnableTFShandler>();
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<CodeGenerateService>();

        return services;
    }
}