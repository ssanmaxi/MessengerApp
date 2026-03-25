namespace messengerApp.Application.SaveMessage.Command;

public record SaveMessageCommand (string text, string roomname, string sender);