using Godot;
using System;
using SpacedRocks.Common;

public class PowerUp : KinematicBody2D
{
    private Area2D consumeArea;
    private int bounce = 1;
    private bool consumed = false;
    private ScreenWrap screenWrap;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 screenSize = Vector2.Zero;
    private AnimationPlayer animationPlayer;
    private readonly Vector2 spriteSize = new Vector2(25, 25);
    
    public override void _Ready()
    {
        consumeArea = GetNode<Area2D>("ConsumeArea");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        var rand = new Random();
        velocity = velocity.Length() > 0
            ? velocity
            : new Vector2(rand.Next(5, 55), 0).Rotated(SpawnPosition.RandomRadian());
        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
        consumeArea.Connect("body_entered", this, nameof(OnConsumeAreaBodyEntered));
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = velocity.Clamped(300);
        var collision = MoveAndCollide(velocity * delta, false, true, false);
        if (collision != null)
        {
            velocity = velocity.Bounce(collision.Normal) * (float)bounce;
        }
        Position = screenWrap.WrappedPosition(Position, spriteSize);
    }

    private void OnConsumeAreaBodyEntered(Node node)
    {
        if (node.Name != "Player")
            return;
        
        if (consumed)
            return;
        
        consumed = true;
        var Player = (Player) node;
        Player.PowerUp();
        animationPlayer.Play("Consumed");

    }

    private void ConsumedAnimationEnd()
    {
        QueueFree();
    }
}
