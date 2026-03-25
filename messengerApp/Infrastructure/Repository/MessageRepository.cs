using messengerApp.Application.Interfaces;
using messengerApp.Domain.Entities;
using messengerApp.Infrastructure.Data;
using messengerApp.Infrastructure.InfrastructureEntity;

namespace messengerApp.Infrastructure.Repository;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _db;

    public MessageRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task SaveAsync(Message msg)
    {
        var entity = new MessagesEntity()
        {
            Text = msg.Text,
            Roomname = msg.Roomname,
            Sender = msg.Sender,
            Date = DateTime.UtcNow
        };

        _db.Messages.Add(entity);
        await _db.SaveChangesAsync();
    }
}