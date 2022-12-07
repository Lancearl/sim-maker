using Godot;

public class WeatherLabel : Label
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready()
    {
        messageBroker.Register(this, nameof(OnWeather));
        Text = string.Empty;
    }

    public void OnWeather(string messageJson) => Text = MessageBrokerService.DeserializeOnWeather(messageJson).Value.ToString();
}

