using Microsoft.EntityFrameworkCore;
using messengerApp.Application.Interfaces;
using messengerApp.Infrastructure.Data;
using messengerApp.Infrastructure.Repository;
using messengerApp.Services;
using messengerApp.Infrastructure.Services;
using StackExchange.Redis;


namespace messengerApp.Infrastructure.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Postgres")));
        
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")));
        
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddSingleton<IOtpStore, RedisOtpStore>();
        services.AddScoped<ILobbyStore, LobbyStore>();
        services.AddScoped<GenerateStrCode>();

        return services;
    }
}