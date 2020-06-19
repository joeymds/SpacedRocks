using Godot;
using System;

public class UI : MarginContainer
{
    private Global global;
    private Label scoreText;
    private Label shieldText;
    private Label level;
    private Label gameOverLabel;
    private Label PowerUpText;
    private Button restartButton;
    
    public override void _Ready()
    {
        global = GetTree().Root.GetNode<Global>("Global");
        scoreText = GetNode<Label>("VBoxContainer/GridContainer/Score/ScoreText");
        shieldText = GetNode<Label>("VBoxContainer/GridContainer/Shield/ShieldText");
        level = GetNode<Label>("VBoxContainer/GridContainer/Level/LevelText");
        gameOverLabel = GetNode<Label>("VBoxContainer/CenterContainer/GameOverLabel");
        restartButton = GetNode<Button>("VBoxContainer/MarginContainer/RestartButton");
        PowerUpText = GetNode<Label>("VBoxContainer/GridContainer/PowerUp/PowerUpText");
        
    }

    public override void _Process(float delta)
    {
        scoreText.Text = global.PlayerScore.ToString();
        shieldText.Text = $"{global.Shield.ToString()}%" ;
        level.Text = global.Level.LevelNumber.ToString();
        gameOverLabel.Visible = global.GameOver;
        restartButton.Visible = global.GameOver;
        PowerUpTextDisplay();
    }

    private void OnReloadButtonPressed()
    {
        global.ResetGame();   
        GetParent().QueueFree();
        GetTree().ChangeScene("res://Scenes/Game.tscn");
    }

    private void PowerUpTextDisplay()
    {
        PowerUpText.Text = global.PowerUpCountDown == 0 ? "0" : global.PowerUpCountDown.ToString();
    }
}
