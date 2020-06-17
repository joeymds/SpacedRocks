using Godot;
using System;
using SpacedRocks.Common;

public class Player : KinematicBody2D
{

    [Export()] public double RotationSpeed = 180;
    [Export()] public int Thrust = 150;
    [Export()] public double Acceleration = 0.1;
    [Export()] public double Friction = 0.01;
    [Export()] public PackedScene _bullet;
    [Export()] public int Shield = 100;
    [Export()] public int ShieldRechargeValue = 3;
    [Export()] public int ShieldRechargeInterval = 3;

    private enum PlayerStates
    {
        disabled,
        normal,
        dead
    }

    private PlayerStates _playerState = PlayerStates.normal;

    private Vector2 _screenSize = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;
    private bool _vulnerable = true; 

    private Global _global;
    private AnimatedSprite _shipSprite;
    private Node _bulletContainer;
    private Position2D Muzzle;
    
    private Light2D _thrustLight;
    private Area2D _shield;
    private AnimationPlayer _shieldPlayer;
    
    private Timer _gunTimer;
    private Timer _shieldRechargeTimer;

    private ScreenWrap _screenWrap;
    private EntityScores _entityScores;
    
    private AudioStreamPlayer _shootSound;
    private AudioStreamPlayer _thrustAudio;
    private AudioStreamPlayer _rechargeBeep;
    private AudioStreamPlayer2D _shieldSound;

    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
        _shipSprite = GetNode<AnimatedSprite>("Ship");
        _bulletContainer = GetNode<Node>("BulletContainer");
        Muzzle = GetNode<Position2D>("Muzzle");
        _shootSound = GetNode<AudioStreamPlayer>("Audio/ShootSound");
        _thrustAudio = GetNode<AudioStreamPlayer>("Audio/ThrustSound");
        _rechargeBeep = GetNode<AudioStreamPlayer>("Audio/RechargeBeep");
        
        _thrustLight = GetNode<Light2D>("ThrustLight");
        _shield = GetNode<Area2D>("Shield");
        _shieldPlayer = GetNode<AnimationPlayer>("Shield/ShieldPlayer");
        
        _gunTimer = GetNode<Timer>("GunTimer");
        _shieldRechargeTimer = GetNode<Timer>("ShieldRechargeTimer");

        _shieldSound = GetNode<AudioStreamPlayer2D>("Shield/AudioStreamPlayer2D");

        _screenSize = GetViewportRect().Size;
        _screenWrap = new ScreenWrap(_screenSize, 8);
        GlobalPosition = _screenSize / 2;

        _shieldRechargeTimer.WaitTime = ShieldRechargeInterval;

        _entityScores = new EntityScores();
        _thrustLight.Enabled = false;
        _global.Shield = Shield;

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
                StateDead();
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
        var moveDirection = new Vector2(0, -1).Rotated(Rotation);
        
        if (Input.IsActionPressed("shoot"))
            if (_gunTimer.TimeLeft == 0)
                Shoot();

        if (Input.IsActionPressed("left"))
        {
            RotationDegrees -= (float)RotationSpeed * delta;
        }

        if (Input.IsActionPressed("right"))
        {
            RotationDegrees += (float) RotationSpeed * delta;
        }

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

    private void StateDead()
    {
        _vulnerable = false;
        _shipSprite.Play("Explode");
    }


    private void PlayerIsNowDead()
    {
        QueueFree();
        GetTree().ChangeScene("res://Scenes/Game.tscn");
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

    private void OnShieldAreaEntered(HitBox hitBox)
    {
        if (_vulnerable == false)
            return;
            
        _shieldSound.Play();
        _vulnerable = false;
        _shieldPlayer.Play("On");
        DamageShield(_entityScores.getRockDamage(hitBox.RockSize));
        
    }

    private void DamageShield(int damageAmount)
    {
        Shield -= damageAmount;
        if (Shield <= 0)
        {
            Shield = 0;
            _global.GameOver = true;
            _playerState = PlayerStates.dead;
            _shieldRechargeTimer.Stop();
        }
        _global.Shield = Shield;
    }
    
    private void ShieldCollisionOver()
    {
        _shieldPlayer.Play("Idle");
        _vulnerable = true;
    }

    private void OnShieldRechargeTimerTimeout()
    {
        if (Shield < 100)
        {
            if (Shield >= 100)
                Shield = 100;
            
            Shield += ShieldRechargeValue;
            _rechargeBeep.Play();
        }
        _global.Shield = Shield;
    }
}
