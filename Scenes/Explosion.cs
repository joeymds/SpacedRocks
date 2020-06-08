using Godot;

public class Explosion : Node2D
{
    private void ExplosionComplete()
    {
        QueueFree();
    }
}
