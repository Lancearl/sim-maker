using System.Collections.Generic;

public class Day
{
    public static int Hours { get; set; } = Constants.DayLength;
    public static List<Day> Days = Utils.LoadConfig().Days ?? defaultDays;
    public static List<Day> defaultDays = new List<Day>()
    {
        new Day()
        {
            Name = Strings.DayMonday,
        },
        new Day()
        {
            Name = Strings.DayTuesday,
        },
        new Day()
        {
            Name = Strings.DayWednesday,
        },
        new Day()
        {
            Name = Strings.DayThursday,
        },
        new Day()
        {
            Name = Strings.DayFriday,
        },
        new Day()
        {
            Name = Strings.DaySaturday,
        },
        new Day()
        {
            Name = Strings.DaySunday,
        },
    };

    public string Name { get; set; }
}
