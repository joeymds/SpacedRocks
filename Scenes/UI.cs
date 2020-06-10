using Godot;
using System;

public class UI : MarginContainer
{
    private Global _global;
    private Label _scoreText;
    private Label _shieldText;
    
    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
        _scoreText = GetNode<Label>("HBoxContainer/Score/ScoreText");
        _shieldText = GetNode<Label>("HBoxContainer/Shield/ShieldText");
    }

    public override void _Process(float delta)
    {
        _scoreText.Text = _global.PlayerScore.ToString();
        _shieldText.Text = _global.ShieldStrength.ToString();
    }
}
