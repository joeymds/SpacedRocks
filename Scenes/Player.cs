using Godot;
using System;
using SpacedRocks.Common;

public class Player : Area2D
{

    [Export()] public double RotationSpeed = 180;
    [Export()] public int Thrust = 150;
    [Export()] public double Acceleration = 0.1;
    [Export()] public double Friction = 0.01;
    [Export()] public PackedScene _bullet;

    private enum PlayerStates
    {
        disabled,
        normal,
        dead
    }

    private PlayerStates _playerState = PlayerStates.normal;

    private Vector2 _screenSize = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;

    private Global _global;
    private AnimatedSprite _shipSprite;
    private AudioStreamPlayer _shootSound;
    private AudioStreamPlayer _thrustAudio;
    private Node _bulletContainer;
    private Position2D Muzzle;
    private Timer _gunTimer;
    private Light2D _thrustLight;

    private ScreenWrap _screenWrap;

    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
        _shipSprite = GetNode<AnimatedSprite>("Ship");
        _bulletContainer = GetNode<Node>("BulletContainer");
        Muzzle = GetNode<Position2D>("Muzzle");
        _gunTimer = GetNode<Timer>("GunTimer");
        _shootSound = GetNode<AudioStreamPlayer>("ShootSound");
        _thrustAudio = GetNode<AudioStreamPlayer>("ThrustSound");
        _thrustLight = GetNode<Light2D>("ThrustLight");

        _screenSize = GetViewportRect().Size;
        _screenWrap = new ScreenWrap(_screenSize, 8);
        GlobalPosition = _screenSize / 2;

        _thrustLight.Enabled = false;
    }

    public override void _Process(float delta)
    {
        if (_global.LevelState == Global.LevelStates.Complete)
        {
            StateDisabled(delta);
            return;
        }
            
        switch (_playerState)
        {
            case PlayerStates.normal:
                StateNormal(delta);
                break;
            case PlayerStates.dead:
                StateDead(delta);
                break;
            case PlayerStates.disabled:
                StateDisabled(delta);
                break;
            default:
                StateDisabled(delta);
                break;
        }
    }

    private void StateNormal(float delta)
    {
        if (Input.IsActionPressed("shoot"))
            if (_gunTimer.TimeLeft == 0)
                Shoot();
        
        if (Input.IsActionPressed("left"))
            RotationDegrees -= (float)RotationSpeed * delta;

        if (Input.IsActionPressed("right"))
            RotationDegrees += (float) RotationSpeed * delta;

        var moveDirection = new Vector2(0, -1).Rotated(Rotation);

        if (Input.IsActionPressed("thrust"))
        {
            _velocity = _velocity.LinearInterpolate(moveDirection, (float) Acceleration);
            _shipSprite.Play("Thrust");
            _thrustAudio.Play();
            _thrustLight.Enabled = true;
        }
        else
        {
            _velocity = _velocity.LinearInterpolate(Vector2.Zero, (float) Friction);
            _shipSprite.Play("Idle");
            _thrustAudio.Stop();
            _thrustLight.Enabled = false;
        }
        
        Position += _velocity * Thrust * delta;
        Position = _screenWrap.WrappedPosition(Position);
    }

    private void StateDisabled(float delta)
    {
        
    }

    private void StateDead(float delta)
    {
        
    }
    
    private void Shoot()
    {
        _gunTimer.Start();
        _shootSound.Play();
        var bullet = (PlayerBullet)_bullet.Instance();
        _bulletContainer.AddChild(bullet);
        bullet.StartAt(Rotation, Muzzle.GlobalPosition);
        _shipSprite.Play("Shoot");
    }
}
