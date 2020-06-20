using Godot;
using System;
using System.Runtime.CompilerServices;
using SpacedRocks.Common;

public class GreenOog : KinematicBody2D
{
    public enum MonsterStates
    {
        Flying, Idle, Hurting, Dying, Dead
    }

    private MonsterStates monsterState = MonsterStates.Idle;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 screenSize = Vector2.Zero;
    private Player player;
    private ScreenWrap screenWrap;
    private Timer shootIntervalTimer;
    private AnimationPlayer animationPlayer;
    private PackedScene ScoreCard;
    private PackedScene OogShoot;

    [Export()] public double Speed = 0.6;
    [Export()] public double Acceleration = 1.0;
    [Export()] public double Friction = 25;
    [Export()] public int Health = 100;
    [Export()] public int ShootInterval = 3;  // How often the monster shoots 
    
    private bool playerInRange = false;

    public override void _Ready()
    {
        shootIntervalTimer = GetNode<Timer>("ShootInterval");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        ScoreCard = (PackedScene) ResourceLoader.Load("res://Scenes/ScoreCard/ScoreCard.tscn");
        OogShoot = (PackedScene) ResourceLoader.Load("res://Scenes/GreenOog/OogShot.tscn");
        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
        shootIntervalTimer.WaitTime = ShootInterval;
        animationPlayer.Play("Fly");
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity = velocity.Clamped(10);
        switch (monsterState)
        {
            case MonsterStates.Idle:
                IdleState(delta);
                break;
            case MonsterStates.Flying:
                FlyingState(delta);
                break;
            case MonsterStates.Hurting:
                HurtingState(delta);
                break;
            default:
                FlyingState(delta);
                break;
        }

        MoveAndCollide(velocity, false, false);
        Position = screenWrap.WrappedPosition(Position);
    }

    private void IdleState(float delta)
    {
        velocity = velocity.MoveToward(Vector2.Zero, (float)Friction * delta);
    }
    
    private void FlyingState(float delta)
    {
        if (!playerInRange)
        {
            shootIntervalTimer.Stop();
            monsterState = MonsterStates.Idle;
            return;
        }

        var direction = GlobalPosition.DirectionTo(player.Position);
        velocity = velocity.MoveToward(direction * (float)Speed, (float)Acceleration * delta);
    }

    private void HurtingState(float delta)
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        var scoreCard = (ScoreCard) ScoreCard.Instance();
        scoreCard.ScoreText = Health.ToString();
        scoreCard.ScoreColour = global::ScoreCard.ScoreColours.Orange;
        scoreCard.StartPosition = GlobalPosition;
        GetParent().AddChild(scoreCard);

        if (Health <= 0)
        {
            QueueFree();
        }
    }
    
    private void OnPlayerDetectionBodyEntered(Node node)
    {
        if (node.Name != "Player") return;
        playerInRange = true;
        player = (Player) node;
        shootIntervalTimer.Start();
        monsterState = MonsterStates.Flying;
    }

    private void OnPlayerDetectionBodyExited(Node node)
    {
        if (node.Name != "Player") return;
        playerInRange = false;
        shootIntervalTimer.Stop();
        monsterState = MonsterStates.Idle;
    }

    private void OnShootIntervalTimeout()
    {
        animationPlayer.Play("Shoot");
        var oogShot = (OogShot) OogShoot.Instance();
        oogShot.StartPosition = GlobalPosition;
        oogShot.MoveDirection = player.GlobalPosition;
        GetParent().AddChild(oogShot);
    }

    private void ShootAnimationComplete()
    {
        animationPlayer.Play("Fly");
    }
}
