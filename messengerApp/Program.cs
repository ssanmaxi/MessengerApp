using messengerApp.Application.Auth.Dependency;
using messengerApp.Application.Dependency;
using messengerApp.Infrastructure.Dependency;
using messengerApp.Presentation.ApiDependencyInjection;
using messengerApp.Presentation.Hubs;
using messengerApp.Presentation.Middlewares;
using messengerApp.Presentation.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApi()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddJwtAuth(builder.Configuration);

builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHub"));

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseStaticFiles();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();