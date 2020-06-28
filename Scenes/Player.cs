using Godot;
using System;
using SpacedRocks.Common;

public class Player : KinematicBody2D
{

    [Export()] public double RotationSpeed = 180;
    [Export()] public int Thrust = 150;
    [Export()] public double Acceleration = 0.1;
    [Export()] public double Friction = 0.01;
    [Export()] public PackedScene bullet;
    [Export()] public int Shield = 100;
    [Export()] public int ShieldRechargeValue = 5;
    [Export()] public int ShieldRechargeInterval = 3;
    [Export()] public double ShootWaitTime = 0.4;
    [Export()] public int PowerUpDuration = 10;

    private enum PlayerStates
    {
        disabled,
        normal,
        dead
    }

    private PlayerStates playerState = PlayerStates.normal;

    private Vector2 screenSize = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;
    
    private bool vulnerable = true;
    private bool powerUpInEffect = false;

    private Global global;
    private Node bulletContainer;
    private Position2D muzzle;
    private AnimationPlayer animationPlayer;

    private Light2D thrustLight;
    private Area2D shield;
    private AnimationPlayer shieldPlayer;
    
    private Timer gunTimer;
    private Timer shieldRechargeTimer;
    private Timer powerUpTick;

    private ScreenWrap screenWrap;
    private EntityScores entityScores;
    
    private AudioStreamPlayer shootSound;
    private AudioStreamPlayer thrustAudio;
    private AudioStreamPlayer rechargeBeep;
    private AudioStreamPlayer2D shieldSound;

    private int PowerUpTickCountDown = 0;

    public override void _Ready()
    {
        global = GetTree().Root.GetNode<Global>("Global");
        bulletContainer = GetNode<Node>("BulletContainer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        muzzle = GetNode<Position2D>("Muzzle");
        shootSound = GetNode<AudioStreamPlayer>("Audio/ShootSound");
        thrustAudio = GetNode<AudioStreamPlayer>("Audio/ThrustSound");
        rechargeBeep = GetNode<AudioStreamPlayer>("Audio/RechargeBeep");
        thrustLight = GetNode<Light2D>("ThrustLight");
        shield = GetNode<Area2D>("Shield");
        shieldPlayer = GetNode<AnimationPlayer>("Shield/ShieldPlayer");
        gunTimer = GetNode<Timer>("GunTimer");
        shieldRechargeTimer = GetNode<Timer>("ShieldRechargeTimer");
        shieldSound = GetNode<AudioStreamPlayer2D>("Shield/AudioStreamPlayer2D");
        
        bullet = (PackedScene) ResourceLoader.Load("res://Scenes/PlayerBullet.tscn");

        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
        GlobalPosition = screenSize / 2;

        shieldRechargeTimer.WaitTime = ShieldRechargeInterval;

        entityScores = new EntityScores();
        thrustLight.Enabled = false;
        global.Shield = Shield;

    }

    public override void _Process(float delta)
    {
        if (global.LevelState == Global.LevelStates.Complete)
        {
            StateDisabled(delta);
            return;
        }
            
        switch (playerState)
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
            if (gunTimer.TimeLeft == 0)
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
            velocity = velocity.LinearInterpolate(moveDirection, (float) Acceleration);
            animationPlayer.Play("Thrust");
            ThrustSound();
            thrustLight.Enabled = true;
        }
        else
        {
            velocity = velocity.LinearInterpolate(Vector2.Zero, (float) Friction);
            animationPlayer.Play("Idle");
            thrustLight.Enabled = false;
        }
        
        Position += velocity * Thrust * delta;
        Position = screenWrap.WrappedPosition(Position);
    }

    private void StateDisabled(float delta)
    {
        
    }

    private void StateDead()
    {
        vulnerable = false;
    }

    private void PlayerIsNowDead()
    {
        QueueFree();
        GetTree().CallGroup("Global", "PlayerDeath");
    }

    private void ThrustSound()
    {
        if (thrustAudio.Playing == false)
            thrustAudio.Play();
    }
    
    private void Shoot()
    {
        gunTimer.Start();
        shootSound.Play();
        var newBullet = (PlayerBullet)bullet.Instance();
        bulletContainer.AddChild(newBullet);
        newBullet.setBulletState(powerUpInEffect
            ? PlayerBullet.BulletStates.Powered
            : PlayerBullet.BulletStates.Standard);
        newBullet.StartAt(Rotation, muzzle.GlobalPosition);
    }

    private void OnShieldAreaEntered(HitBox hitBox)
    {
        if (vulnerable == false)
            return;
        
        shieldSound.Play();
        vulnerable = false;
        shieldPlayer.Play("On");
        DamageShield(hitBox.GetParent().Name == "OogShot"
            ? entityScores.getOogShotDamage()
            : entityScores.getRockDamage(hitBox.RockSize));
    }

    private void DamageShield(int damageAmount)
    {
        Shield -= damageAmount;
        if (Shield <= 0)
        {
            animationPlayer.Play("Die");
            Shield = 0;
            powerUpTick?.Stop();
            playerState = PlayerStates.dead;
            shieldRechargeTimer.Stop();
        }
        else
        {
            GetTree().CallGroup("Global", "UpdateShield", Shield);
        }
    }
    
    private void ShieldCollisionOver()
    {
        shieldPlayer.Play("Idle");
        vulnerable = true;
    }

    private void OnShieldRechargeTimerTimeout()
    {
        if (Shield >= 100) return;

        Shield += ShieldRechargeValue;
        rechargeBeep.Play();
        GetTree().CallGroup("Global", "UpdateShield", Shield);
    }

    public void PowerUp()
    {
        powerUpInEffect = true;
        gunTimer.WaitTime = 0.10f;
        powerUpTick = new Timer { WaitTime = 1, OneShot = false };
        PowerUpTickCountDown += PowerUpDuration;
        powerUpTick.Connect("timeout", this, nameof(OnPowerUpTick));
        AddChild(powerUpTick);
        powerUpTick.Start();
    }
    

    private void OnPowerUpTick()
    {
        GetTree().CallGroup("Global", "SetPowerUpValue", PowerUpTickCountDown);
        if (PowerUpTickCountDown <= 0)
        {
            powerUpTick.Stop();
            gunTimer.WaitTime = 0.4f;
            powerUpInEffect = false;
            return;
        }
        PowerUpTickCountDown -= 1;

    }
}
