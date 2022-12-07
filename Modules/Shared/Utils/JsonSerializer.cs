using Newtonsoft.Json;

// Todo - swap this out for System.Text.Json when the Godot .NET version gets updated.
public static class JsonSerializer
{
    public static string Serialize<T>(T value) => JsonConvert.SerializeObject(value);

    public static T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
}