using Godot;
using System;

public class CountDown : Node
{
    private AnimationPlayer animationPlayer;
    private Label text;
    
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer");
        text = GetNode<Label>("CanvasLayer/Text");
        text.Visible = false;
    }

    public void StartCountDown()
    {
        text.Visible = true;
        animationPlayer.Play("CountDown");
    }
}          
