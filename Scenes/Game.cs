using Godot;

public class Game : Node
{
    private Button startButton;

    public override void _Ready()
    {
        startButton = GetNode<Button>("CanvasLayer/GameUI/CenterContainer/VBoxContainer/MarginContainer/StartButton");
        startButton.Connect("pressed", this, nameof(OnStartButtonPressed));
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeScene("res://Scenes/Main.tscn");
        QueueFree();
    }

}
