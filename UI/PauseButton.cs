using Godot;

public class PauseButton : Button
{
    private bool paused;
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public void _on_Button_button_up()
    {
        GetTree().Paused = !paused;
        messageBroker.SendMessage(new Message<object>()
        {
            Type = MessageType.OnPause,
            Sender = Name
        });
    }
}
