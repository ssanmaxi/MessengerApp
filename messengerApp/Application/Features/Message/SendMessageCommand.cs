namespace messengerApp.Application.Command.SendMessage;

public record SendMessageCommand(string text, string roomname, string sender);