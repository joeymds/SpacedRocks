using Godot;
using System;

public class EndGame : Node
{
    private Global global;
    
    private Button endButton;

    private Label finalScoreText;
    private Label levelScoreText;
    private Label rockScoreText;
    private Label monsterScoreText;
    
    public override void _Ready()
    {
        global = GetTree().Root.GetNode<Global>("Global");
        endButton = GetNode<Button>(
            "CanvasLayer/MarginContainer/CenterContainer/VBoxContainer/VBoxContainer/MarginContainer/Button");
        finalScoreText =
            GetNode<Label>(
                "CanvasLayer/MarginContainer/CenterContainer/VBoxContainer/VBoxContainer/ScoreContainer/FinalScoreText");
        levelScoreText =
            GetNode<Label>(
                "CanvasLayer/MarginContainer/CenterContainer/VBoxContainer/VBoxContainer/LevelContainer/LevelScoreText");
        rockScoreText =
            GetNode<Label>(
                "CanvasLayer/MarginContainer/CenterContainer/VBoxContainer/VBoxContainer/RocksContainer/RockScoreText");
        monsterScoreText =
            GetNode<Label>(
                "CanvasLayer/MarginContainer/CenterContainer/VBoxContainer/VBoxContainer/MonstersContainer/MonsterScoreText");

        finalScoreText.Text = global.PlayerScore.ToString();
        levelScoreText.Text = global.LevelCount.ToString();
        monsterScoreText.Text = global.MonsterCount.ToString();
        rockScoreText.Text = global.RockCount.ToString();

        endButton.Connect("pressed", this, nameof(EndButtonClicked));
    }

    private void EndButtonClicked()
    {
        GetTree().ChangeScene("res://Scenes/Game.tscn");
    }

}
