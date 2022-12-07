public class Date
{
    public Day Day { get; set; }
    public int DayNumber { get; set; }
    public Season Season { get; set; } // Month
    public int Year { get; set; }

    public void AddDay()
    {
        var currentDayIndex = Day.Days.FindIndex(x => x.Name == Day.Name);
        var dayCount = Day.Days.Count;
        Day = dayCount > currentDayIndex + 1 ? Day.Days[currentDayIndex + 1] : Day.Days[0];
        DayNumber = DayNumber >= Season.Days ? 1 : DayNumber + 1;

        // Calculate if the new date is in a different year.
        if (DayNumber == 1
            && Season.Name == Season.Seasons[Season.Seasons.Count - 1].Name)
            Year += 1;
    }

    public void SubtractDay()
    {
        var dayCount = Day.Days.Count;
        var currentDayIndex = Day.Days.FindIndex(x => x.Name == Day.Name);
        var currentSeasonIndex = Season.Seasons.FindIndex(x => x.Name == Season.Name);
        var previousSeason = currentSeasonIndex - 1 >= 0 ? Season.Seasons[currentSeasonIndex - 1] : Season.Seasons[Season.Seasons.Count - 1];
        Day = currentDayIndex - 1 >= 0 ? Day.Days[currentDayIndex - 1] : Day.Days[dayCount - 1];
        DayNumber = DayNumber - 1 <= 0 ? previousSeason.Days : DayNumber - 1;

        // Calculate if the new date is in a different year.
        if (DayNumber == Season.Days
            && previousSeason.Name == Season.Seasons[Season.Seasons.Count - 1].Name
        )
            Year = Year > 1 ? Year - 1 : Year;
    }

    public override string ToString() => $"{DayNumber} {Day.Name}, Year {Year}";

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Date a, Date b) => a.Day == b.Day && a.DayNumber == b.DayNumber && a.Year == b.Year;

    public static bool operator !=(Date a, Date b) => a.Day != b.Day || a.DayNumber != b.DayNumber || a.Year != b.Year;

    public static bool operator >(Date a, Date b) =>
        (a.Year > b.Year) ||
        ((Season.Seasons.FindIndex(x => x.Name == a.Season.Name) > Season.Seasons.FindIndex(x => x.Name == b.Season.Name)) && a.Year >= b.Year) ||
        ((a.DayNumber > b.DayNumber) && (Season.Seasons.FindIndex(x => x.Name == a.Season.Name) >= (Season.Seasons.FindIndex(x => x.Name == b.Season.Name))) && (a.Year >= b.Year));

    public static bool operator <(Date a, Date b) =>
        (a.Year < b.Year) ||
        (Season.Seasons.FindIndex(x => x.Name == a.Season.Name) < (Season.Seasons.FindIndex(x => x.Name == b.Season.Name)) && a.Year <= b.Year) ||
        (a.DayNumber < b.DayNumber && (Season.Seasons.FindIndex(x => x.Name == a.Season.Name) <= Season.Seasons.FindIndex(x => x.Name == b.Season.Name)) && a.Year <= b.Year);

    public static bool operator >=(Date a, Date b) =>
        ((a.DayNumber >= b.DayNumber) && (Season.Seasons.FindIndex(x => x.Name == a.Season.Name) >= (Season.Seasons.FindIndex(x => x.Name == b.Season.Name))) && (a.Year >= b.Year));

    public static bool operator <=(Date a, Date b) =>
        (a.DayNumber <= b.DayNumber && (Season.Seasons.FindIndex(x => x.Name == a.Season.Name) <= Season.Seasons.FindIndex(x => x.Name == b.Season.Name)) && a.Year <= b.Year);

    public static Date DeepClone(Date a) =>
        new Date()
        {
            Day = new Day()
            {
                Name = a.Day.Name,
            },
            DayNumber = a.DayNumber,
            Season = new Season()
            {
                Name = a.Season.Name,
                Days = a.Season.Days,
            },
            Year = a.Year,
        };
}
