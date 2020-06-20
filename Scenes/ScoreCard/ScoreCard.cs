using Godot;
using System;

public class ScoreCard : Node2D
{
    public string ScoreText = "TEST";
    public Vector2 StartPosition = Vector2.Zero;

    private Label scoreTextLabel;
    
    public override void _Ready()
    {
        scoreTextLabel = GetNode<Label>("ScoreText");
        scoreTextLabel.Text = ScoreText;
        GlobalPosition = new Vector2(StartPosition.x, StartPosition.y - 30);
        GD.Print(ScoreText);
    }

    private void EndScoreCard()
    {
        QueueFree();
    }

}
