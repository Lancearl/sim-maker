using System.Collections.Generic;

public class Season
{
    public static List<Season> Seasons = Utils.LoadConfig().Seasons ?? defaultSeasons;
    private static List<Season> defaultSeasons = new List<Season>()
    {
        new Season()
        {
            Name = Strings.SeasonSpring,
        },
        new Season()
        {
            Name = Strings.SeasonSummer,
        },
        new Season()
        {
            Name = Strings.SeasonAutumn,
        },
        new Season()
        {
            Name = Strings.SeasonWinter,
        },
    };
    public string Name { get; set; }
    public int Days { get; set; } = Constants.SeasonLength;

    public static Dictionary<WeatherType, Range> GetWeathers(Season season)
    {
        var springWeathers = new Dictionary<WeatherType, Range>()
        {
            { WeatherType.Rain, new Range(1, 30) }, // 30%
            { WeatherType.Clear, new Range(31, 70) }, // 40%
            { WeatherType.Overcast, new Range(71, 95) }, // 25%
            { WeatherType.Storm, new Range(96, 100) }, // 5%
        };
        switch (season.Name)
        {
            case Strings.SeasonSpring:
                return springWeathers;
            case Strings.SeasonSummer:
                return new Dictionary<WeatherType, Range>()
                {
                    { WeatherType.Rain, new Range(1, 5) }, // 5%
                    { WeatherType.Storm, new Range(6, 10) }, // 5%
                    { WeatherType.Clear, new Range(11, 80) }, // 70%
                    { WeatherType.Overcast, new Range(81, 100) }, // 20%
                };
            case Strings.SeasonAutumn:
                return new Dictionary<WeatherType, Range>()
                {
                    { WeatherType.Snow, new Range(1, 5) }, // 5%
                    { WeatherType.Rain, new Range(6, 35) }, // 30%
                    { WeatherType.Storm, new Range(36, 40) }, // 5%
                    { WeatherType.Clear, new Range(41, 80) }, // 40%
                    { WeatherType.Overcast, new Range(81, 100) }, // 20%
                };
            case Strings.SeasonWinter:
                return new Dictionary<WeatherType, Range>()
                {
                    { WeatherType.Snow, new Range(1, 35) }, // 35%
                    { WeatherType.Blizzard, new Range(36, 45) }, // 10%
                    { WeatherType.Clear, new Range(46, 70) }, // 25%
                    { WeatherType.Overcast, new Range(71, 100) }, // 30%
                };
            default:
                return springWeathers;
        }
    }
}
