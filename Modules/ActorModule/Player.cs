using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D, IPersist
{
    [Export] private float speed = 100;
    [Export] private NodePath animationPlayerPath;
    [Export] private NodePath animationTreePath;

    public CharacterFacing CurrentFacing { get; set; }

    private const float MaxSpeed = 200;
    private AnimationPlayer _animationPlayer;
    private AnimationPlayer animationPlayer
    {
        get
        {
            if (_animationPlayer == null)
            {
                _animationPlayer = GetNode<AnimationPlayer>(animationPlayerPath);
            }
            return _animationPlayer;
        }
        set
        {
            _animationPlayer = value;
        }
    }
    private AnimationTree _animationTree;
    private AnimationTree animationTree
    {
        get
        {
            if (_animationTree == null)
            {
                _animationTree = GetNode<AnimationTree>(animationTreePath);
                _animationTree.Active = true;
            }
            return _animationTree;
        }
        set
        {
            _animationTree = value;
        }
    }
    private AnimationNodeStateMachinePlayback animationState = null;
    private IMessageBrokerService messageBroker => GetNode<IMessageBrokerService>(Strings.MessageBrokerNodePath);
    private bool paused = false;
    private PlayerState playerState = PlayerState.Move;
    private Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        messageBroker.Register(this, nameof(OnPause));
        animationState = animationTree.Get(Strings.AnimationParameters) as AnimationNodeStateMachinePlayback;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (CanMove())
        {
            switch (playerState)
            {
                case PlayerState.Move:
                    MoveState();
                    break;
            }
        }
    }

    public void OnPause(string messageJson) => paused = !paused;

    public void Load(Dictionary<string, string> data)
    {
        if (data.ContainsKey(Strings.KeyPlayerCurrentFacing))
        {
            Enum.TryParse<CharacterFacing>(data[Strings.KeyPlayerCurrentFacing], out var currentFacing);
            CurrentFacing = currentFacing;
            animationTree.Set($"parameters/{Strings.AnimationStateIdle}/blend_position", GetCurrentFacingVector(CurrentFacing));
        }

        if (data.ContainsKey(Strings.KeyPlayerCurrentPositionX) && data.ContainsKey(Strings.KeyPlayerCurrentPositionY))
        {
            var positionX = JsonSerializer.Deserialize<float>(data[Strings.KeyPlayerCurrentPositionX]);
            var positionY = JsonSerializer.Deserialize<float>(data[Strings.KeyPlayerCurrentPositionY]);
            GlobalPosition = new Vector2(positionX, positionY);
        }
    }

    public Dictionary<string, object> Save() => new Dictionary<string, object>()
    {
        { Strings.KeyPlayerCurrentFacing, CurrentFacing.ToString() },
        { Strings.KeyPlayerCurrentPositionX, GlobalPosition.x },
        { Strings.KeyPlayerCurrentPositionY, GlobalPosition.y }
    };

    private bool CanMove() => !paused;

    private void MoveState()
    {
        var inputVector = GetInputVector();
        SetCurrentFacing(inputVector);
        if (inputVector != Vector2.Zero)
        {
            animationTree.Set($"parameters/{Strings.AnimationStateIdle}/blend_position", inputVector);
            animationTree.Set($"parameters/{Strings.AnimationStateWalk}/blend_position", inputVector);
            animationState.Travel(Strings.AnimationStateWalk);
            velocity = inputVector * speed;
            velocity = velocity.LimitLength(MaxSpeed);
        }
        else
        {
            animationState.Travel(Strings.AnimationStateIdle);
            velocity = Vector2.Zero;
        }

        velocity = MoveAndSlide(velocity);
    }

    private Vector2 GetInputVector()
    {
        var inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength(Strings.Right) - Input.GetActionStrength(Strings.Left);
        inputVector.y = Input.GetActionStrength(Strings.Down) - Input.GetActionStrength(Strings.Up);
        return inputVector.Normalized();
    }

    /// <summary>
    /// Convert normalized inputVector to facing enum. Extra conditions to handle diagonals i.e. player is pressing down 2 directional keys.
    /// Diagonal right can be:
    /// 0.7071068f, -0.7071068f
    /// 0.7071068f, 0.7071068f
    /// Diagonal left can be:
    /// -0.7071068f, 0.7071068f
    /// -0.7071068f, -0.7071068f
    /// </summary>
    /// <param name="inputVector"></param>
    private void SetCurrentFacing(Vector2 inputVector)
    {
        if (inputVector != Vector2.Zero)
        {
            if (inputVector == Vector2.Up)
                CurrentFacing = CharacterFacing.Up;
            else if (inputVector == Vector2.Left || (inputVector.x < 0 && inputVector.x > -1))
                CurrentFacing = CharacterFacing.Left;
            else if (inputVector == Vector2.Right || (inputVector.x > 0 && inputVector.x < 1))
                CurrentFacing = CharacterFacing.Right;
            else
                CurrentFacing = CharacterFacing.Down;
        }
    }

    private Vector2 GetCurrentFacingVector(CharacterFacing currentFacing)
    {
        switch (currentFacing)
        {
            case CharacterFacing.Down:
                return Vector2.Down;
            case CharacterFacing.Left:
                return Vector2.Left;
            case CharacterFacing.Right:
                return Vector2.Right;
            default:
                return Vector2.Up;
        }
    }
}
