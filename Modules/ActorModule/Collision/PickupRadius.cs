using Godot;

public class PickupRadius : Area2D
{
    [Export] private NodePath radiusShapePath;

    private CollisionShape2D _collisionShape;
    public CollisionShape2D collisionShape
    {
        get => _collisionShape == null ? _collisionShape = GetNode<CollisionShape2D>(radiusShapePath) : _collisionShape;
        set => _collisionShape = value;
    }

    /// <summary>
    /// Body will only ever be an Item because the pickup radius only looks to that layer.
    /// </summary>
    /// <param name="body"></param>
    public void _on_PickupRadius_body_entered(Node body)
    {
        // TODO
    }

    public void _on_PickupRadius_body_exited(Node body)
    {
        // TODO
    }
}