namespace messengerApp.Infrastructure.InfrastructureEntity;

public class MessagesEntity
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Text { get; set; }
    public string Roomname { get; set; }
    public DateTime Date { get; set; }
}