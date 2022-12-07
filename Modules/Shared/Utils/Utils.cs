using Godot;

public static class Utils
{
    /// <summary>
    /// Recursively return all paths for nodes of a given type T that are children of a given node.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="matchingNodes">Method will add any found nodes to this enumerable as it iterates.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The passed enumerable with any found nodes of type T in it.</returns>
    public static Godot.Collections.Array<string> GetNodePathsOfType<T>(Node node, Godot.Collections.Array<string> array)
    {
        foreach (Node n in node.GetChildren())
        {
            if (n.GetChildCount() > 0)
            {
                if (n is T)
                    array.Add(n.GetPath());

                GetNodePathsOfType<T>(n, array);
            }
            else
            {
                if (n is T)
                    array.Add(n.GetPath());
            }
        }

        return array;
    }

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
