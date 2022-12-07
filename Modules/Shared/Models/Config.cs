using System.Collections.Generic;

public class Config
{
    public List<Day> Days { get; set; }
    public List<Season> Seasons { get; set; }
    public int? HourLength { get; set; }
    public int? DayLength { get; set; }
    public int? SeasonLength { get; set; }
    public int? DawnTimeHour { get; set; }
    public int? DayTimeHour { get; set; }
    public int? DuskTimeHour { get; set; }
    public int? NightTimeHour { get; set; }
}
