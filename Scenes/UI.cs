using Godot;
using System;

public class UI : MarginContainer
{
    private Global _global;
    private Label _scoreText;
    private Label _shieldText;
    private Label _level;
    private Label _gameOverLabel;
    private Button _restartButton;
    
    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
        _scoreText = GetNode<Label>("VBoxContainer/HBoxContainer/Score/ScoreText");
        _shieldText = GetNode<Label>("VBoxContainer/HBoxContainer/Shield/ShieldText");
        _level = GetNode<Label>("VBoxContainer/HBoxContainer/Level/LevelText");
        _gameOverLabel = GetNode<Label>("VBoxContainer/CenterContainer/GameOverLabel");
        _restartButton = GetNode<Button>("VBoxContainer/MarginContainer/RestartButton");
    }

    public override void _Process(float delta)
    {
        _scoreText.Text = _global.PlayerScore.ToString();
        _shieldText.Text = $"{_global.Shield.ToString()}%" ;
        _level.Text = _global.Level.LevelNumber.ToString();
        _gameOverLabel.Visible = _global.GameOver;
        _restartButton.Visible = _global.GameOver;
    }

    private void OnReloadButtonPressed()
    {
        _global.ResetGame();   
        GetParent().QueueFree();
        GetTree().ChangeScene("res://Scenes/Game.tscn");
    }
}
