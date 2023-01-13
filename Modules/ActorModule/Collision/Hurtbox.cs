using Godot;

public class Hurtbox : Area2D
{
    [Signal] public delegate void invincibility_started();
    [Signal] public delegate void invincibility_ended();

    [Export] private NodePath timerPath;
    [Export] private NodePath collisionShapePath;

    private Timer _timer;
    private Timer timer
    {
        get
        {
            if (_timer == null)
            {
                _timer = GetNode<Timer>(timerPath);
            }
            return _timer;
        }
        set
        {
            _timer = value;
        }
    }
    private CollisionShape2D _collisionShape;
    private CollisionShape2D collisionShape
    {
        get
        {
            if (_collisionShape == null)
            {
                _collisionShape = GetNode<CollisionShape2D>(collisionShapePath);
            }
            return _collisionShape;
        }
        set
        {
            _collisionShape = value;
        }
    }


    private bool _invincible = false;
    public bool invincible
    {
        get { return _invincible; }
        set { SetInvincible(value); }
    }

    private void SetInvincible(bool value)
    {
        _invincible = value;
        if (invincible)
        {
            EmitSignal(nameof(invincibility_started));
        }
        else
        {
            EmitSignal(nameof(invincibility_ended));
        }
    }

    public void StartInvincibility(float duration)
    {
        this.invincible = true;
        timer.Start(duration);
    }

    public void CreateHitEffect()
    {
        // TODO
    }

    private void _on_Timer_timeout()
    {
        this.invincible = false;
    }

    private void _on_Hurtbox_invincibility_started()
    {
        // Switch off the collision box so the player stops getting hit.
        collisionShape.SetDeferred("disabled", true);
    }

    private void _on_Hurtbox_invincibility_ended()
    {
        // Switch the collision box back on.
        collisionShape.SetDeferred("disabled", false);
    }
}
