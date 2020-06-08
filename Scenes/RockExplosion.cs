using Godot;
using System;

public class RockExplosion : Node2D
{
    private AudioStreamPlayer2D _explosionSound;
    private Timer _timer;
    
    public override void _Ready()
    {
        _explosionSound = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", this, nameof(ExplosionEnd));
        
        _explosionSound.Stream = GetRockExplosionSound();
        _explosionSound.Play();
        _timer.Start();
    }
    
    private static AudioStream GetRockExplosionSound()
    {
        var rndRockImage = new Random();
        var soundIndex = rndRockImage.Next(1, 3);
        return (AudioStream)GD.Load($"res://SFX/explosions/rock-explosion-{soundIndex}.ogg");
    }

    private void ExplosionEnd()
    {
        QueueFree();
    }
    
}
