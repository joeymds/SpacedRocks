using Godot;
using System;

public class CountDown : Node
{
    private AnimationPlayer _animationPlayer;
    private Label _text;
    
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("MarginContainer/CenterContainer/AnimationPlayer");
        _text = GetNode<Label>("MarginContainer/CenterContainer/Text");
        _text.Visible = false;
    }

    public void StartCountDown()
    {
        _text.Visible = true;
        _animationPlayer.Play("CountDown");
    }
}          
