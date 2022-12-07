using Godot;

public class LoadButton : Button
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_LoadButton_button_up()
    {
        messageBroker.SendMessage(new Message<string>()
        {
            Type = MessageType.LoadGame,
            Value = "", // The file name can be passed in.
            Sender = Name
        });
    }
}
