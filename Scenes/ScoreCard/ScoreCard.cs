using Godot;
using System;

public class ScoreCard : Node2D
{
    public string ScoreText = "TEST";

    public enum ScoreColours
    {
        Blue, Orange
    }

    public ScoreColours ScoreColour = ScoreColours.Blue;
    public Vector2 StartPosition = Vector2.Zero;

    private Label scoreTextLabel;
    private readonly Color blueColor = new Color(0.5f, 0.68f, 1, 1);
    private readonly Color orangeColor = new Color(0.93f, 0.60f, 0.08f, 1);
    public override void _Ready()
    {
        scoreTextLabel = GetNode<Label>("ScoreText");
        scoreTextLabel.Text = ScoreText;
        scoreTextLabel.Set("custom_colors/font_color", ScoreColour == ScoreColours.Blue ? blueColor : orangeColor);
        GlobalPosition = new Vector2(StartPosition.x, StartPosition.y - 30);
    }

    private void EndScoreCard()
    {
        QueueFree();
    }

}
