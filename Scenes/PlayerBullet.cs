
using Godot;
using SpacedRocks.Common;

public class PlayerBullet : Area2D
{
    public enum BulletStates
    {
        Standard, Powered
    }

    private Global global;
    private Sprite sprite;
    private Vector2 velocity = Vector2.Zero;
    private Light2D bulletLight;
    private Timer lifeTime;
    
    private readonly Color standardColor = new Color(0.36f, 0.83f, 0.93f, 1.0f);
    private readonly Color poweredColor = new Color(0.96f, 0.92f, 0.48f, 1.0f);

    private BulletStates bulletState = BulletStates.Standard;

    [Export()] private int speed = 600;

    public override void _Ready()
    {
        global = GetTree().Root.GetNode<Global>("Global");
        sprite = GetNode<Sprite>("Sprite");
        lifeTime = GetNode<Timer>("Lifetime");
        bulletLight = GetNode<Light2D>("BulletLight");
    }

    public override void _Process(float delta)
    {
        Position += velocity * delta;
        switch (bulletState)
        {
            case BulletStates.Standard:
                bulletLight.Color = standardColor;
                sprite.Texture = global.NormalBulletTexture;
                break;
            case BulletStates.Powered:
                bulletLight.Color = poweredColor;
                sprite.Texture = global.PoweredBulletTexture;
                break;
        }
    }

    public void StartAt(float direction, Vector2 position)
    {
        Rotation = direction;
        Position = new Vector2(position);
        velocity = new Vector2(speed, 0).Rotated(direction - Mathf.Pi/2);
    }

    public void setBulletState(BulletStates state)
    {
        bulletState = state;
        lifeTime.WaitTime = bulletState == BulletStates.Standard ? 0.4f : 0.6f;
    }
    
    private void OnPlayerBulletBodyEntered(ref Rock rock)
    {
        rock.Explode(rock.rockSize, rock.rockVelocity, velocity.Normalized());
        var entityScores = new EntityScores();
        var rockScore = entityScores.getRockScore(rock.rockSize);
        GetTree().CallGroup("Global", "RockHit", rockScore);
        GetTree().CallGroup("Camera", "RockHit");
        QueueFree();
    }

    private void OnLifetimeTimeout()
    {
        QueueFree();
    }
}
