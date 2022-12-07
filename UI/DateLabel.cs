using Godot;

public class DateLabel : Label
{
    private Date defaultDate = new Date()
    {
        Day = Day.Days[0],
        DayNumber = 1,
        Year = 1,
    };
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready()
    {
        messageBroker.Register(this, new string[] { nameof(OnDay), nameof(OnYear) });
        Text = defaultDate.ToString();
    }

    public void OnDay(string messageJson) => Text = MessageBrokerService.DeserializeOnDay(messageJson).Value.ToString();

    public void OnYear(string messageJson) => Text = MessageBrokerService.DeserializeOnYear(messageJson).Value.ToString();
}
