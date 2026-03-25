namespace messengerApp.Application.DTO;

public class MessageDTO
{
    public string Sender { get; set; }
    public string Text { get; set; }
    public string Roomname { get; set; }
    public DateTime Date { get; set; }
}