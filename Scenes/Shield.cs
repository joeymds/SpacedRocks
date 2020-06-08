using Godot;

public class Shield : Area2D
{
    private CollisionShape2D _shieldCollision;
    private Area2D _detector;
    private Sprite _shield;
    private Timer _shieldTimer;
    private bool _active;

    public override void _Ready()
    {
        _shieldCollision = GetNode<CollisionShape2D>("ShieldCollision");
        _shield = GetNode<Sprite>("Sprite");
        _detector = GetNode<Area2D>("Detector");
        _shieldTimer = GetNode<Timer>("ShieldTimer");
        
        _shieldCollision.Visible = false;
        _shield.Visible = false;
        _active = false;

        _detector.Connect("body_entered", this, nameof(OnEnemyEntered));
        _shieldTimer.Connect("timeout", this, nameof(OnShieldTimerTimeout));
        Connect("body_entered", this, nameof(OnShieldCollision));
    }

    private void EnableShield()
    {
        if (_active) return;
        _active = true;
        _shield.Visible = true;
        _shieldCollision.Visible = true;
        _shieldTimer.Start();
    }

    private void OnEnemyEntered(Rock rock)
    {
        GD.Print(" -> " + rock.Name);
        EnableShield();
    }

    private void OnShieldTimerTimeout()
    {
        _active = false;
        _shield.Visible = false;
        _shieldCollision.Visible = false;
    }

    private void OnShieldCollision(Rock rock)
    {
        rock._velocity = rock._velocity.Bounce(rock._velocity.Normalized());
    }
}
