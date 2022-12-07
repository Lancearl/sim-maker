using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Co-ordinates weather and determines when to start a new weather event.
/// </summary>
public class WeatherController : Node2D, IPersist
{
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);
    private bool paused;
    private Time time;
    private Date date = new Date()
    {
        Day = Day.Days[0],
        DayNumber = 1,
        Season = Season.Seasons[0],
        Year = 1
    };
    private Weather weather;

    public override void _Ready() => messageBroker.Register(this, new string[] { nameof(OnMinute), nameof(OnDay), nameof(OnYear), nameof(OnSeason), nameof(OnPause) });

    public void OnMinute(string messageJson)
    {
        if (!paused)
        {
            time = MessageBrokerService.DeserializeOnMinute(messageJson).Value;
            if (WeatherEnded())
                StartNewWeather();
        }
    }

    public void OnDay(string messageJson) => date = MessageBrokerService.DeserializeOnDay(messageJson).Value;

    public void OnYear(string messageJson) => date = MessageBrokerService.DeserializeOnYear(messageJson).Value;

    public void OnSeason(string messageJson) => date.Season = MessageBrokerService.DeserializeOnSeason(messageJson).Value;

    public void OnPause(string messageJson) => paused = !paused;

    public Dictionary<string, object> Save() => new Dictionary<string, object>()
    {
        { Strings.KeyCurrentWeather, JsonSerializer.Serialize(weather) }
    };

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyCurrentWeather))
            weather = JsonSerializer.Deserialize<Weather>(data[Strings.KeyCurrentWeather]);

        AddWeather();
        SendOnWeather();
    }

    private bool WeatherEnded() => weather == null || (time > weather.End.Time && date >= weather.End.Date);

    /// <summary>
    /// Pick the new weather, determine its duration and add it to the scene.
    /// </summary>
    private void StartNewWeather()
    {
        var random = new Random();
        var numberBetweenOneAndOneHundred = random.Next(1, 101);
        foreach (KeyValuePair<WeatherType, Range> pair in Season.GetWeathers(date.Season))
        {
            if (pair.Value.InRange(numberBetweenOneAndOneHundred))
            {
                weather = new Weather()
                {
                    Type = pair.Key,
                    Start = new DateTime()
                    {
                        Date = date,
                        Time = time,
                    },
                    End = CalculateWeatherEndTime(GetDurationInHours()),
                };
                AddWeather();
                SendOnWeather();
                break;
            }
        }
    }

    /// <summary>
    /// Calculate when the date and time of the weather will end.
    /// </summary>
    /// <returns>the current date and time with the duration in hours added</returns>
    private DateTime CalculateWeatherEndTime(int durationInHours)
    {
        var t = Time.DeepClone(time);
        var d = Date.DeepClone(date);
        for (int i = 0; i < durationInHours; i++)
        {
            t.Hour++;
            if (t.Hour == 0)
                // ! Warning - this will break if the number of days for all seasons is not uniform. Need to calculate the current season and save it to the date before adding a day.
                d.AddDay();
        }

        return new DateTime()
        {
            Date = d,
            Time = t,
        };
    }

    /// <summary>
    /// Return a duration for weather events weighted towards the higher numbers.
    /// </summary>
    /// <returns>Numbers higher than 12 about 75% of the time.</returns>
    private int GetDurationInHours()
    {
        var random = new Random();
        var randomNumber1 = random.Next(1, Day.Hours + 1);
        var randomNumber2 = random.Next(1, Day.Hours + 1);
        return randomNumber1 > randomNumber2 ? randomNumber1 : randomNumber2;
    }

    private void AddWeather()
    {
        ClearWeather();
        var weatherScene = GD.Load<PackedScene>(Weather.weatherScenePaths[weather.Type]).Instance<WeatherNode>();
        AddChild(weatherScene);
    }

    private void ClearWeather()
    {
        foreach (Node child in GetChildren())
            RemoveChild(child);
    }

    private void SendOnWeather() => messageBroker.SendMessage(new Message<Weather>()
    {
        Type = MessageType.OnWeather,
        Value = weather,
        Sender = Name
    });
}
