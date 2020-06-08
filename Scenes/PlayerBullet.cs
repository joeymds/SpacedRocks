using Godot;

public class PlayerBullet : Area2D
{
    private Vector2 _velocity = Vector2.Zero;

    [Export()] private int _speed = 1000;

    public override void _Process(float delta)
    {
        Position += _velocity * delta;
    }

    public void StartAt(float direction, Vector2 position)
    {
        Rotation = direction;
        Position = new Vector2(position);
        _velocity = new Vector2(_speed, 0).Rotated(direction - Mathf.Pi/2);
    }

    private void OnPlayerBulletBodyEntered(ref Rock rock)
    {
        rock.Explode(rock._rockSize, rock._velocity, _velocity.Normalized());
        QueueFree();
    }

    private void OnLifetimeTimeout()
    {
        QueueFree();
    }
}
