using Godot;

public class HourSubtractButton : Button
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_HourSubtractButton_button_up()
    {
        messageBroker.SendMessage(new Message<int>()
        {
            Type = MessageType.AddHour,
            Value = -1,
            Sender = Name
        });
    }
}
