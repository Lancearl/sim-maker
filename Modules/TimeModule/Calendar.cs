using System.Collections.Generic;
using Godot;

/// <summary>
/// Keeps track of the current Date and year.
/// </summary>
public class Calendar : Node, IPersist
{
    private Time time = new Time();
    private Date date = new Date()
    {
        Day = Day.Days[0],
        DayNumber = 1,
        Season = Season.Seasons[0],
        Year = 1,
    };
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready() => messageBroker.Register(this, new string[] { nameof(OnHour), nameof(OnSeason), nameof(AddDay) });

    public void AddDay(string messageJson)
    {
        var message = MessageBrokerService.DeserializeAddDay(messageJson);
        UpdateDay(message.Value);
    }

    public void OnHour(string messageJson)
    {
        var message = MessageBrokerService.DeserializeOnHour(messageJson);
        var previousHour = time.Hour;
        time = message.Value;
        if (previousHour == Day.Hours - 1 && time.Hour == 0)
            UpdateDay(1);
        else if (previousHour == 0 && time.Hour == Day.Hours - 1)
            UpdateDay(-1);
    }

    public void OnSeason(string messageJson)
    {
        var message = MessageBrokerService.DeserializeOnSeason(messageJson);
        date.Season = message.Value;
    }

    public Dictionary<string, object> Save() => new Dictionary<string, object>
    {
        { Strings.KeyCurrentDate, JsonSerializer.Serialize(date) }
    };

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyCurrentDate))
            date = JsonSerializer.Deserialize<Date>(data[Strings.KeyCurrentDate].ToString());

        SendOnDay();
    }

    private void SendOnDay() => messageBroker.SendMessage(new Message<Date>
    {
        Type = MessageType.OnDay,
        Value = date,
        Sender = this.Name
    });

    private void SendOnYear() => messageBroker.SendMessage(new Message<Date>
    {
        Type = MessageType.OnYear,
        Value = date,
        Sender = Name
    });

    private void UpdateDay(int amount)
    {
        if (amount == 0)
            return;

        var days = amount > 0 ? amount : -amount;
        for (int i = 0; i < days; i++)
        {
            var previousYear = date.Year;
            if (amount > 0)
                date.AddDay();
            else
                date.SubtractDay();

            SendOnDay();
            if (date.Year != previousYear)
                SendOnYear();
        }
    }
}
