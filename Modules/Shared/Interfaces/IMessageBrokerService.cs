using Godot;
using System.Collections.Generic;

public interface IMessageBrokerService
{
    void SendMessage<T>(Message<T> message);
    void Register<T>(T caller, string signalType) where T : Node;
    void Register<T>(T caller, IEnumerable<string> signals) where T : Node;
    void Unregister<T>(T caller, string signalType) where T : Node;
    void Unregister<T>(T caller, IEnumerable<string> signals) where T : Node;
}
