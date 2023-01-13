using Godot;

public class SeasonLabel : Label
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready()
    {
        messageBroker.Register(this, nameof(OnSeason));
        Text = Season.Seasons[0].Name;
    }

    public void OnSeason(string messageJson) => Text = MessageBrokerService.DeserializeOnSeason(messageJson).Value.Name;
}
