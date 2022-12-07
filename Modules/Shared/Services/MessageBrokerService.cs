using Godot;
using System.Collections.Generic;

/// <summary>
/// The MessageBroker bridges communication between modules and objects in the game using signals.
/// Its design mimics a simplified event streaming platform. 
/// MessageBroker must be instantiated as a Autoload/Singleton. See here for details: https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html
/// </summary>
public class MessageBrokerService : Node, IMessageBrokerService
{
    // All signals deliver a serialized JSON Message object.
    [Signal] public delegate void OnMinute(string message);
    [Signal] public delegate void OnHour(string message);
    [Signal] public delegate void OnDay(string message);
    [Signal] public delegate void OnDayPhase(string message);
    [Signal] public delegate void OnSeason(string message);
    [Signal] public delegate void OnYear(string message);
    [Signal] public delegate void OnPause(string message);
    [Signal] public delegate void OnWeather(string message);

    // Debug
    [Signal] public delegate void AddHour(string message);
    [Signal] public delegate void AddDay(string message);

    // Serialisation
    [Signal] public delegate void SaveGame(string message);
    [Signal] public delegate void LoadGame(string message);
    [Signal] public delegate void OnSave(string message);
    [Signal] public delegate void OnLoad(string message);

    public static List<string> Registrations = new List<string>();

    /// <summary>
    /// Send a message. Value of a message can be any serialized object.
    /// </summary>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    public void SendMessage<T>(Message<T> message) => EmitSignal(message.Type.ToString(), JsonSerializer.Serialize<Message<T>>(message) ?? null);

    /// <summary>
    /// Connect the caller to the signal and register them.
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="signalType"></param>
    /// <typeparam name="T"></typeparam>
    public void Register<T>(T caller, string signalType) where T : Node
    {
        if (!IsConnected(signalType, caller, signalType))
            Connect(signalType, caller, signalType);

        var entry = $"{caller.Name}_{signalType}";
        if (!Registrations.Contains(entry))
            Registrations.Add(entry);
    }

    /// <summary>
    /// Connect a caller to a range of signals and register them.
    /// </summary>
    /// <param name="callerInfo"></param>
    /// <typeparam name="T"></typeparam>
    public void Register<T>(T caller, IEnumerable<string> signals) where T : Node
    {
        foreach (var signal in signals)
            this.Register(caller, signal);
    }

    /// <summary>
    /// Disconnect the caller from the signal and unregister it.
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="signalType"></param>
    /// <typeparam name="T"></typeparam>
    public void Unregister<T>(T caller, string signalType) where T : Node
    {
        if (IsConnected(signalType, caller, signalType))
            Disconnect(signalType, caller, signalType);

        var entry = $"{caller.Name}_{signalType}";
        if (Registrations.Contains(entry))
            Registrations.Remove(entry);
    }

    /// <summary>
    /// Disconnect a caller from a range of signals and unregister them.
    /// </summary>
    /// <param name="callerInfo"></param>
    /// <typeparam name="T"></typeparam>
    public void Unregister<T>(T caller, IEnumerable<string> signals) where T : Node
    {
        foreach (var signal in signals)
            this.Unregister(caller, signal);
    }


    #region Deserialize signal messages
    /// <summary>
    /// Method that all deserialize calls pass through, in order to implement any shared behaviour.
    /// </summary>
    /// <param name="messageJson"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static Message<T> DeserializeMessage<T>(string messageJson) => JsonSerializer.Deserialize<Message<T>>(messageJson);
    public static Message<Time> DeserializeOnMinute(string messageJson) => DeserializeMessage<Time>(messageJson);
    public static Message<Time> DeserializeOnHour(string messageJson) => DeserializeMessage<Time>(messageJson);
    public static Message<Date> DeserializeOnDay(string messageJson) => DeserializeMessage<Date>(messageJson);
    public static Message<DayPhase> DeserializeOnDayPhase(string messageJson) => DeserializeMessage<DayPhase>(messageJson);
    public static Message<Season> DeserializeOnSeason(string messageJson) => DeserializeMessage<Season>(messageJson);
    public static Message<Date> DeserializeOnYear(string messageJson) => DeserializeMessage<Date>(messageJson);
    public static Message<object> DeserializeOnPause(string messageJson) => DeserializeMessage<object>(messageJson);
    public static Message<Weather> DeserializeOnWeather(string messageJson) => DeserializeMessage<Weather>(messageJson);
    public static Message<int> DeserializeAddHour(string messageJson) => DeserializeMessage<int>(messageJson);
    public static Message<int> DeserializeAddDay(string messageJson) => DeserializeMessage<int>(messageJson);
    public static Message<string> DeserializeSaveGame(string messageJson) => DeserializeMessage<string>(messageJson); // string = the save file name.
    public static Message<string> DeserializeLoadGame(string messageJson) => DeserializeMessage<string>(messageJson); // string = the save file name.
    public static Message<Dictionary<string, object>> DeserializeOnSave(string messageJson) => DeserializeMessage<Dictionary<string, object>>(messageJson); // Dictionary = the data that gets serialised.
    public static Message<Dictionary<string, string>> DeserializeOnLoad(string messageJson) => DeserializeMessage<Dictionary<string, string>>(messageJson); // Dictionary = the data that has been deserialised from a save file.
    #endregion
}