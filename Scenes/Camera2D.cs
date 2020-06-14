using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    private AnimationPlayer _animationPlayer;
    
    public override void _Ready()
    {
        AddToGroup("Camera");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private void RockHit()
    {
        _animationPlayer.Play("Hit");
    }
}
