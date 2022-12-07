using System.Collections.Generic;
using Godot;

/// <summary>
/// Updates seasons in game.
/// </summary>
public class SeasonController : Node, IPersist
{
    private Date date = new Date()
    {
        Day = Day.Days[0],
        DayNumber = 1,
        Year = 1,
    };
    private Season season = Season.Seasons[0];
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready() => messageBroker.Register(this, nameof(OnDay));

    public void OnDay(string messageJson)
    {
        var message = MessageBrokerService.DeserializeOnDay(messageJson);
        var previousDate = date;
        date = message.Value;
        if (SeasonEnded(date, previousDate))
            UpdateSeason(date, previousDate);
    }

    public Dictionary<string, object> Save() => new Dictionary<string, object>()
    {
        { Strings.KeyCurrentSeason, JsonSerializer.Serialize(season) }
    };

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyCurrentSeason))
            season = JsonSerializer.Deserialize<Season>(data[Strings.KeyCurrentSeason]);

        SendOnSeason();
    }

    private bool SeasonEnded(Date currentDay, Date previousDay) =>
        IsNextSeason(currentDay, previousDay) ||
        IsPreviousSeason(currentDay, previousDay);

    private void UpdateSeason(Date currentDay, Date previousDay)
    {
        var currentSeasonIndex = Season.Seasons.FindIndex(x => x.Name == season.Name);
        if (IsNextSeason(currentDay, previousDay))
            season = Season.Seasons.Count > currentSeasonIndex + 1 ? Season.Seasons[currentSeasonIndex + 1] : Season.Seasons[0];
        else if (IsPreviousSeason(currentDay, previousDay))
            season = currentSeasonIndex - 1 >= 0 ? Season.Seasons[currentSeasonIndex - 1] : Season.Seasons[Season.Seasons.Count - 1];

        SendOnSeason();
    }

    private bool IsNextSeason(Date currentDay, Date previousDay) => previousDay.DayNumber == season.Days && currentDay.DayNumber == 1;

    private bool IsPreviousSeason(Date currentDay, Date previousDay) => currentDay.DayNumber == season.Days && previousDay.DayNumber == 1;

    private void SendOnSeason() => messageBroker.SendMessage(new Message<Season>()
    {
        Type = MessageType.OnSeason,
        Value = season,
        Sender = Name,
    });
}
