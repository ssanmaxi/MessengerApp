using messengerApp.Domain.Entities;
namespace messengerApp.Application.Interfaces;

public interface IMessageRepository
{
    Task SaveAsync(Message msg);
}