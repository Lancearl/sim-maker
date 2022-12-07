using Godot;
using System.Collections.Generic;

/// <summary>
/// Advances and tracks in game time.
/// </summary>
public class Clock : Timer, IPersist
{
    private Time time = new Time();
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);
    private bool paused = false;

    public override void _Ready() => messageBroker.Register(this, new string[] { nameof(OnPause), nameof(AddHour) });

    public void _on_Clock_timeout()
    {
        if (!paused)
            UpdateMinute(1);
    }

    public void AddHour(string messageJson)
    {
        var message = MessageBrokerService.DeserializeAddHour(messageJson);
        UpdateHour(message.Value);
    }

    public void OnPause(string messageJson) => paused = !paused;

    public Dictionary<string, object> Save() => new Dictionary<string, object>()
    {
        { Strings.KeyCurrentTime, JsonSerializer.Serialize(time) }
    };

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyCurrentTime))
            time = JsonSerializer.Deserialize<Time>(data[Strings.KeyCurrentTime]);

        SendOnMinute();
        SendOnHour();
    }

    /// <summary>
    /// Safely handles multiple hours being passed (i.e. passing 8 hours would give you the correct time).
    /// Pass negative hours to go backwards in time.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public void UpdateHour(int amount)
    {
        if (amount == 0)
            return;

        var hours = amount > 0 ? amount : -amount;
        for (int i = 0; i < hours; i++)
        {
            var previousHour = time.Hour;
            if (amount > 0)
                time.Hour++;
            else
                time.Hour--;

            if (time.Hour != previousHour)
                SendOnHour();
        }
    }

    /// <summary>
    /// Safely handles multiple minutes being passed (i.e. passing 240 minutes would give you the correct time).
    /// Pass negative minutes to go backwards in time.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public void UpdateMinute(int amount)
    {
        if (amount == 0)
            return;

        var minutes = amount > 0 ? amount : -amount;
        for (int i = 0; i < minutes; i++)
        {
            var previousMinute = time.Minute;
            if (amount > 0)
            {
                time.Minute++;
                SendOnMinute();
                if ((previousMinute + 1) >= Constants.HourLength)
                    SendOnHour();
            }
            else
            {
                time.Minute--;
                SendOnMinute();
                if ((previousMinute - 1) < 0)
                    SendOnHour();
            }
        }
    }

    private void SendOnMinute() => messageBroker.SendMessage(new Message<Time>()
    {
        Type = MessageType.OnMinute,
        Value = time,
        Sender = this.Name
    });

    private void SendOnHour() => messageBroker.SendMessage(new Message<Time>()
    {
        Type = MessageType.OnHour,
        Value = time,
        Sender = this.Name
    });
}