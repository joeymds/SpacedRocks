using Godot;

public class Game : Node
{
    private Button _startButton;

    public override void _Ready()
    {
        _startButton = GetNode<Button>("CanvasLayer/GameUI/CenterContainer/VBoxContainer/MarginContainer/StartButton");
        _startButton.Connect("pressed", this, nameof(OnStartButtonPressed));
    }

    private void OnStartButtonPressed()
    {
        QueueFree();
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

}
