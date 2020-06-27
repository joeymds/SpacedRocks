using Godot;
using System;

public class OogShot : KinematicBody2D
{
    private Vector2 velocity = Vector2.Zero;
    private Vector2 direction;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D audioPlayer;
    private Timer lifeSpan;

    [Export()] public double Speed = 1.8;
    [Export()] public double Acceleration = 5;
    [Export()] public double Friction = 10;
    [Export()] public int LifeSpan = 4;
    [Export()] public Vector2 MoveDirection = Vector2.Zero;
    [Export()] public Vector2 StartPosition = Vector2.Zero;

    public override void _Ready()
    {
        lifeSpan = GetNode<Timer>("LifeSpan");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        GlobalPosition = new Vector2(StartPosition.x, StartPosition.y + 10);
        lifeSpan.WaitTime = 4;
        lifeSpan.OneShot = true;
        lifeSpan.Connect("timeout", this, nameof(ExplodeShot));
        lifeSpan.Start();
        direction = GlobalPosition.DirectionTo(MoveDirection);
    }
    
    public override void _Process(float delta)
    {
        velocity = velocity.MoveToward(direction * (float)Speed, (float)Acceleration * delta);
        MoveAndCollide(velocity, false, false);
    }

    private void OnHitBoxAreaEntered(Area2D area)
    {
        lifeSpan.Stop();
        animationPlayer.Play("explode");
    }
        
    private void ExplodeShot()
    {
        lifeSpan.Stop();
        animationPlayer.Play("explode");
    }
    
    private void LifeEnded()
    {
        QueueFree();
    }
}
