using Godot;
using System;
using SpacedRocks.Common;

public class GreenOog : KinematicBody2D
{
    public enum MonsterStates
    {
        Flying, Idle, Shooting, Hurting, Dying, Dead
    }

    private MonsterStates monsterState = MonsterStates.Idle;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 screenSize = Vector2.Zero;
    private Player player;
    private ScreenWrap screenWrap;

    [Export()] public int Speed = 1;
    [Export()] public double Acceleration = 1.0;
    [Export()] public double Friction = 25;
    [Export()] public int Health = 100;
    
    private bool playerInRange = false;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = velocity.Clamped(10);
        switch (monsterState)
        {
            case MonsterStates.Idle:
                IdleState(delta);
                break;
            case MonsterStates.Flying:
                FlyingState(delta);
                break;
            default:
                FlyingState(delta);
                break;
        }

        MoveAndCollide(velocity, false, false);
        Position = screenWrap.WrappedPosition(Position);
    }

    private void IdleState(float delta)
    {
        velocity = velocity.MoveToward(Vector2.Zero, (float)Friction * delta);
    }
    
    private void FlyingState(float delta)
    {
        if (!playerInRange)
        {
            monsterState = MonsterStates.Idle;
            return;
        }
        
        var direction = GlobalPosition.DirectionTo(player.Position);
        velocity = velocity.MoveToward(direction * Speed, (float)Acceleration * delta);
    }

    private void OnPlayerDetectionBodyEntered(Node node)
    {
        if (node.Name != "Player") return;
        playerInRange = true;
        player = (Player) node;
        monsterState = MonsterStates.Flying;
    }

    private void OnPlayerDetectionBodyExited(Node node)
    {
        if (node.Name != "Player") return;
        playerInRange = false;
    }
}
