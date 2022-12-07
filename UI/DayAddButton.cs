using Godot;

public class DayAddButton : Button
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_DayAddButton_button_up()
    {
        messageBroker.SendMessage(new Message<int>()
        {
            Type = MessageType.AddDay,
            Value = 1,
            Sender = Name
        });
    }
}
