using Godot;

public class Pause : Control
{
    public override void _Ready()
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            var pauseState = !GetTree().Paused;
            GetTree().Paused = pauseState;
            Visible = pauseState;
        }
    }
}
