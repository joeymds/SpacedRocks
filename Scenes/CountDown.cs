using Godot;
using System;

public class CountDown : Node2D
{
    private AnimationPlayer _animationPlayer;
    
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("MarginContainer/CenterContainer/AnimationPlayer");
    }

    public void StartCountDown()
    {
        _animationPlayer.Play("CountDown");
    }
}    
