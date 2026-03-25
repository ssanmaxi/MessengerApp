namespace messengerApp.Domain.Entities;

public class Message
{
    public string Text { get; set; }
    public string Roomname { get; set; }
    public string Sender { get; set; }
    public DateTime CreatedAt { get; set; }

    public Message(string text, string roomname, string sender)
    {
        if (string.IsNullOrEmpty(text)) throw new ArgumentException("text has nothing");
        if (string.IsNullOrEmpty(roomname)) throw new ArgumentException("no roomname");
        if (string.IsNullOrEmpty(sender)) throw new ArgumentException("no sender");

        Text = text;
        Roomname = roomname;
        Sender = sender;
        CreatedAt = DateTime.UtcNow;
    }
}