using messengerApp.Application.DTO;
namespace messengerApp.Application.Auth.Register;

public record RegisterCommand(string Email, string Password, string Name, string ProfileName, int Age);
