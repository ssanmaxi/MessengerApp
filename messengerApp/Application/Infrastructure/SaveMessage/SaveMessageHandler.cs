using messengerApp.Application.Interfaces;
using messengerApp.Application.SaveMessage.Command;
using messengerApp.Domain.Entities;

namespace messengerApp.Application.SaveMessage.Handler;

public class SaveMessageHandler
{
    private readonly IMessageRepository _mr;

    public SaveMessageHandler(IMessageRepository MessageRepository)
    {
        _mr = MessageRepository;
    }

    public async Task Handle(SaveMessageCommand command)
    {
        var msg = new Message
        (
            command.text,
            command.roomname,
            command.sender
        );

        await _mr.SaveAsync(msg);
    }
}