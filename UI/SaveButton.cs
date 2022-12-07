using Godot;

public class SaveButton : Button
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_SaveButton_button_up()
    {
        messageBroker.SendMessage(new Message<string>()
        {
            Type = MessageType.SaveGame,
            Value = "", // The file name can be passed in.
            Sender = Name
        });
    }
}
