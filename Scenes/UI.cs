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
    private VBoxContainer PowerUpContainer;
    
    public override void _Ready()
    {
        global = GetTree().Root.GetNode<Global>("Global");
        scoreText = GetNode<Label>("VBoxContainer/GridContainer/Score/ScoreText");
        shieldText = GetNode<Label>("VBoxContainer/GridContainer/Shield/ShieldText");
        level = GetNode<Label>("VBoxContainer/GridContainer/Level/LevelText");
        gameOverLabel = GetNode<Label>("VBoxContainer/CenterContainer/GameOverLabel");
        restartButton = GetNode<Button>("VBoxContainer/MarginContainer/RestartButton");
        PowerUpText = GetNode<Label>("VBoxContainer/GridContainer/PowerUp/PowerUpText");
        PowerUpContainer = GetNode<VBoxContainer>("VBoxContainer/GridContainer/PowerUp");
        PowerUpContainer.Visible = false;
    }

    public override void _Process(float delta)
    {
        shieldText.Text = (global.Shield > 100) ? $"{100}%" : $"{global.Shield}%";
        scoreText.Text = global.PlayerScore.ToString();
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
        if (global.Level.NumberOfPowerUps > 0)
            PowerUpContainer.Visible = true;
        PowerUpText.Text = global.PowerUpCountDown == 0 ? "0" : global.PowerUpCountDown.ToString();
    }
}
