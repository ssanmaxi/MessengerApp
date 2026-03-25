using messengerApp.Application.Command.SendMessage;
using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
namespace messengerApp.Application.Messages;

public class SendMessageHandler
{
    private readonly IMessageRepository _mr;

    public SendMessageHandler(IMessageRepository mr)
    {
        _mr = mr;
    }

    public async Task Handle(SendMessageCommand command)
    {
        var msg = new Message(
            command.text,
            command.roomname,
            command.sender
        );

        await _mr.SaveAsync(msg);
    }
}