using Godot;
using SpacedRocks.Common;

public class PlayerBullet : Area2D
{
    public enum BulletType
    {
        Standard, Powered
    }
    
    private Vector2 velocity = Vector2.Zero;
    private Light2D bulletLight;
    
    public BulletType bulletType = BulletType.Standard;

    [Export()] private int speed = 600;

    public override void _Ready()
    {
        bulletLight = GetNode<Light2D>("BulletLight");
    }

    public override void _Process(float delta)
    {
        Position += velocity * delta;
        switch (bulletType)
        {
            case BulletType.Standard:
                //bulletLight.Color = new Color(131, 239, 253, 255);
                break;
            case BulletType.Powered:
                //bulletLight.Color = new Color(239, 111, 94, 255);
                break;
        }
    }

    public void StartAt(float direction, Vector2 position)
    {
        Rotation = direction;
        Position = new Vector2(position);
        velocity = new Vector2(speed, 0).Rotated(direction - Mathf.Pi/2);
    }

    private void OnPlayerBulletBodyEntered(ref Rock rock)
    {
        rock.Explode(rock._rockSize, rock._velocity, velocity.Normalized());
        var entityScores = new EntityScores();
        var rockScore = entityScores.getRockScore(rock._rockSize);
        GetTree().CallGroup("Global", "RockHit", rockScore);
        GetTree().CallGroup("Camera", "RockHit");
        QueueFree();
    }

    private void OnLifetimeTimeout()
    {
        QueueFree();
    }
}
