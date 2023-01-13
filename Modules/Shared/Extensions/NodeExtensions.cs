using Godot;
using System.Collections.Generic;

public static class NodeExtensions
{
    /// <summary>
    /// Get children of the given node of a given type, if any.
    /// </summary>
    /// <param name="node"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetChildrenOfType<T>(this Node node)
    {
        var children = new List<T>();
        foreach (var child in node.GetChildren())
            if (child is T)
                children.Add((T)child);

        return children;
    }

    public static T GetChildOfType<T>(this Node node)
    {
        foreach (var child in node.GetChildren())
            if (child is T)
                return (T)child;

        return default(T);
    }

    /// <summary>
    /// Recursively return all paths for nodes of a given type T that are children of a given node.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="matchingNodes">Method will add any found nodes to this enumerable as it iterates.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The passed enumerable with any found nodes of type T in it.</returns>
    public static Godot.Collections.Array<string> GetNodePathsOfType<T>(this Node node, Godot.Collections.Array<string> array)
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
}
