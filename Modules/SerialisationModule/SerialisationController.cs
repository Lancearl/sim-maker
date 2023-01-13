using Godot;
using System.Collections.Generic;
using System.Linq;

public class SerialisationController : Node
{
    private string defaultFileName = Strings.DefaultFileName;
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);

    public override void _Ready() => messageBroker.Register(this, new string[] { nameof(SaveGame), nameof(LoadGame) });

    public void SaveGame(string messageJson)
    {
        var message = MessageBrokerService.DeserializeSaveGame(messageJson);
        var fileName = !string.IsNullOrWhiteSpace(message.Value) ? message.Value : defaultFileName;
        var saveData = new Dictionary<string, object>();
        var array = new Godot.Collections.Array<string>();
        var persistNodePaths = GetTree().Root.GetNodePathsOfType<IPersist>(array);
        foreach (var nodePath in persistNodePaths)
        {
            var persistNode = GetNode(nodePath) as IPersist;
            persistNode.Save().ToList().ForEach(dict =>
            {
                if (saveData.ContainsKey(dict.Key))
                    saveData[dict.Key] = dict.Value;
                else
                    saveData.Add(dict.Key, dict.Value);
            });
        }

        using (var saveFile = new File())
        {
            saveFile.Open($"{Strings.SaveDirectory}/{fileName}{Strings.SaveExtension}", File.ModeFlags.Write);
            saveFile.StoreLine(JsonSerializer.Serialize(saveData));
        }

        messageBroker.SendMessage(new Message<object>()
        {
            Type = MessageType.OnSave,
            Value = saveData,
            Sender = Name
        });
    }

    public void LoadGame(string messageJson)
    {
        var message = MessageBrokerService.DeserializeLoadGame(messageJson);
        var fileName = !string.IsNullOrWhiteSpace(message.Value) ? message.Value : defaultFileName;
        var filePath = $"{Strings.SaveDirectory}/{fileName}{Strings.SaveExtension}";
        using (var saveFile = new File())
        {
            if (!saveFile.FileExists(filePath))
                return;

            saveFile.Open(filePath, File.ModeFlags.Read);
            var saveData = new Dictionary<string, string>();
            while (saveFile.GetPosition() < saveFile.GetLen())
                saveData = JsonSerializer.Deserialize<Dictionary<string, string>>(saveFile.GetLine());

            var array = new Godot.Collections.Array<string>();
            var persistNodePaths = GetTree().Root.GetNodePathsOfType<IPersist>(array);
            foreach (var nodePath in persistNodePaths)
            {
                var persistNode = GetNode(nodePath) as IPersist;
                persistNode.Load(saveData);
            }

            messageBroker.SendMessage(new Message<object>()
            {
                Type = MessageType.OnLoad,
                Value = saveData,
                Sender = Name
            });
        }
    }
}
