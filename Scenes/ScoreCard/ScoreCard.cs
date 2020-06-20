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
    
    public override void _Ready()
    {
        scoreTextLabel = GetNode<Label>("ScoreText");
        scoreTextLabel.Text = ScoreText;
        if (ScoreColour == ScoreColours.Blue)
            scoreTextLabel.Set("custom_colors/font_color", new Color(0.5f, 0.68f, 1, 1));
        else
            scoreTextLabel.Set("custom_colors/font_color", new Color(0.93f, 0.60f, 0.08f, 1));
        GlobalPosition = new Vector2(StartPosition.x, StartPosition.y - 30);
        GD.Print(ScoreText);
    }

    private void EndScoreCard()
    {
        QueueFree();
    }

}
