public class Message<T>
{
    public MessageType Type { get; set; }
    public T Value { get; set; }
    public string Sender { get; set; }
}