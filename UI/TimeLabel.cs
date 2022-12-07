using Godot;

public class TimeLabel : Label
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready() => messageBroker.Register(this, new string[] { nameof(OnMinute), nameof(OnHour) });

    public void OnMinute(string messageJson) => this.Text = MessageBrokerService.DeserializeOnMinute(messageJson).Value.ToString();

    public void OnHour(string messageJson) => this.Text = MessageBrokerService.DeserializeOnHour(messageJson).Value.ToString();
}
