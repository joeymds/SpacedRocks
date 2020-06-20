using Godot;
using System;

public class OogShot : KinematicBody2D
{
    private Vector2 velocity = Vector2.Zero;
    private Vector2 direction;
    private Timer lifeSpan;
    
    [Export()] public double Speed = 1.5;
    [Export()] public double Acceleration = 5;
    [Export()] public double Friction = 10;
    [Export()] public int LifeSpan = 4;
    [Export()] public Vector2 MoveDirection = Vector2.Zero;
    [Export()] public Vector2 StartPosition = Vector2.Zero;

    public override void _Ready()
    {
        GlobalPosition = new Vector2(StartPosition.x, StartPosition.y + 10);
        lifeSpan = GetNode<Timer>("LifeSpan");
        lifeSpan.WaitTime = 4;
        lifeSpan.OneShot = true;
        lifeSpan.Connect("timeout", this, nameof(LifeEnded));
        lifeSpan.Start();
        direction = GlobalPosition.DirectionTo(MoveDirection);
    }
    
    public override void _Process(float delta)
    {
        velocity = velocity.MoveToward(direction * (float)Speed, (float)Acceleration * delta);
        MoveAndCollide(velocity, false, false);
    }

    private void LifeEnded()
    {
        QueueFree();
    }
}
