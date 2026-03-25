using Microsoft.Extensions.DependencyInjection;
namespace messengerApp.Presentation.ApiDependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSignalR();
        
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());
        });

        return services;
    }
}