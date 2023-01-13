using Godot;

public static class Utils
{
    /// <summary>
    /// Load the config file for the game.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Config LoadConfig(string path = Strings.ConfigFilePath)
    {
        Config config = null;
        using (var saveFile = new File())
        {
            saveFile.Open(path, File.ModeFlags.Read);
            config = JsonSerializer.Deserialize<Config>(saveFile.GetAsText());
        }

        return config;
    }
}
