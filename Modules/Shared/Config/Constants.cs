public static class Constants
{
    public static int HourLength = Utils.LoadConfig().HourLength ?? 60;
    public static int DayLength = Utils.LoadConfig().DayLength ?? 24;
    public static int SeasonLength = Utils.LoadConfig().SeasonLength ?? 60;
    public static int DawnTimeHour = Utils.LoadConfig().DawnTimeHour ?? 6;
    public static int DayTimeHour = Utils.LoadConfig().DayTimeHour ?? 7;
    public static int DuskTimeHour = Utils.LoadConfig().DuskTimeHour ?? 17;
    public static int NightTimeHour = Utils.LoadConfig().NightTimeHour ?? 18;
}
