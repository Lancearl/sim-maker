using System.Collections.Generic;
using Godot;

/// <summary>
/// Changes the colour of tilesets in time with each phase of the day cycle.
/// </summary>
public class DayController : CanvasModulate, IPersist
{
    [Export] public Color dawnColour, dayColour, duskColour, nightColour;
    [Export] public float dawnTransitionTime, dayTransitionTime, duskTransitionTime, nightTransitionTime;

    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);
    private float elapsedTime = 0;
    private bool paused = false;
    private Time time = new Time();
    private DayPhase dayPhase = DayPhase.Night;
    private Dictionary<int, DayPhase> phases = new Dictionary<int, DayPhase>()
    {
        { Constants.DawnTimeHour, DayPhase.Dawn },
        { Constants.DayTimeHour, DayPhase.Day },
        { Constants.DuskTimeHour, DayPhase.Dusk },
        { Constants.NightTimeHour, DayPhase.Night },
    };

    public override void _Ready()
    {
        messageBroker.Register(this, new string[] { nameof(OnHour), nameof(OnPause) });
        SetProcess(false);
        Color = nightColour;
    }

    public override void _Process(float delta)
    {
        if (!paused)
        {
            elapsedTime += delta;
            UpdateColour(elapsedTime);
        }
    }

    public void OnPause(string messageJson) => paused = !paused;

    public void OnHour(string messageJson)
    {
        var message = MessageBrokerService.DeserializeOnHour(messageJson);
        var previousDayPhase = dayPhase;
        time = message.Value;
        UpdateCurrentDayPhase();
        if (previousDayPhase != dayPhase)
        {
            elapsedTime = 0;
            SetProcess(true);
            SendOnDayPhase();
        }
    }

    public Dictionary<string, object> Save() => new Dictionary<string, object>()
    {
        { Strings.KeyCurrentDayPhase, JsonSerializer.Serialize(dayPhase) }
    };

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyCurrentDayPhase))
            dayPhase = JsonSerializer.Deserialize<DayPhase>(data[Strings.KeyCurrentDayPhase]);

        SendOnDayPhase();
        SetProcess(true);
    }

    private void UpdateCurrentDayPhase()
    {
        if (phases.ContainsKey(time.Hour))
            dayPhase = phases[time.Hour];
    }

    private void UpdateColour(float time)
    {
        (Color startColour, Color endColour, float duration) = GetLerpColourValues();
        if (time < duration)
            Color = LerpColour(startColour, endColour, duration);
        else
            SetProcess(false);
    }

    private Color LerpColour(Color startColour, Color endColour, float duration)
    {
        var remaining = elapsedTime / duration;
        return new Color(
            Lerp(startColour.r, endColour.r, remaining),
            Lerp(startColour.g, endColour.g, remaining),
            Lerp(startColour.b, endColour.b, remaining),
            Lerp(startColour.a, endColour.a, remaining));
    }

    private (Color startColour, Color endColour, float duration) GetLerpColourValues()
    {
        var startColour = nightColour;
        var endColour = dawnColour;
        var duration = dawnTransitionTime;
        switch (dayPhase)
        {
            case DayPhase.Day:
                startColour = dawnColour;
                endColour = dayColour;
                duration = dayTransitionTime;
                break;
            case DayPhase.Dusk:
                startColour = dayColour;
                endColour = duskColour;
                duration = duskTransitionTime;
                break;
            case DayPhase.Night:
                startColour = duskColour;
                endColour = nightColour;
                duration = nightTransitionTime;
                break;
        }

        return (startColour, endColour, duration);
    }

    /// <summary>
    /// Interpolate a float value between a and b divided by t.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private float Lerp(float a, float b, float t) => (1 - t) * a + t * b;

    private void SendOnDayPhase() => messageBroker.SendMessage<DayPhase>(new Message<DayPhase>()
    {
        Type = MessageType.OnDayPhase,
        Value = dayPhase,
        Sender = Name,
    });
}
