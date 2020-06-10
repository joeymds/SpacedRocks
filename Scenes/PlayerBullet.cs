using Godot;
using SpacedRocks.Common;

public class PlayerBullet : Area2D
{
    private Vector2 _velocity = Vector2.Zero;

    [Export()] private int _speed = 600;

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
        var entityScores = new EntityScores();
        var rockScore = entityScores.getRockScore(rock._rockSize);
        GetTree().CallGroup("Global", "UpdateScore", rockScore);
        QueueFree();
    }

    private void OnLifetimeTimeout()
    {
        QueueFree();
    }
}
