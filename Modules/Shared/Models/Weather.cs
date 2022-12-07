using System.Collections.Generic;

public class Weather
{
    public WeatherType Type { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public static Dictionary<WeatherType, string> weatherScenePaths = new Dictionary<WeatherType, string>
    {
        { WeatherType.Blizzard, Strings.BlizzardWeatherScenePath },
        { WeatherType.Clear, Strings.ClearWeatherScenePath },
        { WeatherType.Overcast, Strings.OvercastWeatherScenePath },
        { WeatherType.Rain, Strings.RainWeatherScenePath },
        { WeatherType.Snow, Strings.SnowWeatherScenePath },
        { WeatherType.Storm, Strings.StormWeatherScenePath },
    };

    public override string ToString() => $"{Type} - {End}";
}
