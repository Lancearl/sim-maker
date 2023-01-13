public static class Strings
{
    #region Animation
    public const string AnimationParameters = "parameters/playback";
    public const string AnimationStateIdle = "Idle";
    public const string AnimationStateWalk = "Walk";
    #endregion

    #region Inputs
    public const string Left = "ui_left";
    public const string Right = "ui_right";
    public const string Up = "ui_up";
    public const string Down = "ui_down";
    public const string Debug = "ui_debug";
    #endregion

    #region Paths
    public const string MessageBrokerNodePath = "/root/MessageBroker";
    public const string ConfigFilePath = "res://config.json";
    public const string SwitchRegistryFilePath = "res://switchRegistry.json";
    #endregion

    #region Weathers
    public const string BlizzardWeatherScenePath = "res://Modules/WeatherModule/Blizzard.tscn";
    public const string ClearWeatherScenePath = "res://Modules/WeatherModule/Clear.tscn";
    public const string OvercastWeatherScenePath = "res://Modules/WeatherModule/Overcast.tscn";
    public const string RainWeatherScenePath = "res://Modules/WeatherModule/Rain.tscn";
    public const string SnowWeatherScenePath = "res://Modules/WeatherModule/Snow.tscn";
    public const string StormWeatherScenePath = "res://Modules/WeatherModule/Storm.tscn";
    #endregion

    #region Names
    public const string SeasonSpring = "Spring";
    public const string SeasonSummer = "Summer";
    public const string SeasonAutumn = "Autumn";
    public const string SeasonWinter = "Winter";
    public const string DayMonday = "Monday";
    public const string DayTuesday = "Tuesday";
    public const string DayWednesday = "Wednesday";
    public const string DayThursday = "Thursday";
    public const string DayFriday = "Friday";
    public const string DaySaturday = "Saturday";
    public const string DaySunday = "Sunday";
    #endregion

    #region Serialisation
    public const string SaveDirectory = "res://Saves";
    public const string DefaultFileName = "save-game";
    public const string KeyCurrentTime = "CurrentTime";
    public const string KeyCurrentDate = "CurrentDate";
    public const string KeyCurrentSeason = "CurrentSeason";
    public const string KeyCurrentWeather = "CurrentWeather";
    public const string KeyCurrentDayPhase = "CurrentDayPhase";
    public const string KeyPlayerCurrentFacing = "PlayerCurrentFacing";
    public const string KeyPlayerCurrentPositionX = "PlayerCurrentPositionX";
    public const string KeyPlayerCurrentPositionY = "PlayerCurrentPositionY";
    #endregion

    #region Extensions
    public const string SaveExtension = ".sav";
    #endregion
}
