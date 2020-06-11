using Godot;
using System;
using System.ComponentModel.Design;

public class Game : Node2D
{
    private Global _global;
    private Button _startButton;

    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
        _startButton = GetNode<Button>("GameUI/CenterContainer/VBoxContainer/StartButton");
        _startButton.Connect("pressed", this, nameof(OnStartButtonPressed));
    }

    private void OnStartButtonPressed()
    {
        QueueFree();
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

}
