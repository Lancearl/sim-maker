using Godot;

public class HourAddButton : Button
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_HourAddButton_button_up()
    {
        messageBroker.SendMessage(new Message<int>()
        {
            Type = MessageType.AddHour,
            Value = 1,
            Sender = Name
        });
    }
}
