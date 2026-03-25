namespace messengerApp.Presentation.Middlewares;

public class NotFoundException : Exception
{
    public NotFoundException(string message, object key) : base($"Entity {message} with key {key} not found"){}
}