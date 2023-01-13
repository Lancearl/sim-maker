using Godot;

public class DebugUI : Control
{
    private bool _isShowing;
    public bool IsShowing
    {
        get => _isShowing;
        set
        {
            _isShowing = value;
            Visible = value;
        }
    }

    public override void _Ready() => IsShowing = false;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased(Strings.Debug))
            IsShowing = !IsShowing;
    }
}
