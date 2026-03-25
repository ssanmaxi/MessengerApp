namespace messengerApp.Application.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}