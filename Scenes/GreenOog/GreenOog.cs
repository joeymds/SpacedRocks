using Godot;
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
    private Timer hurtTimer;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D hurtAudio2D;
    private AudioStreamPlayer2D deathAudio2D;
    private PackedScene ScoreCard;
    private PackedScene OogShoot;

    [Export()] public double Speed = 0.6;
    [Export()] public double Acceleration = 1.0;
    [Export()] public double Friction = 25;
    [Export()] public int Health = 100;
    [Export()] public int ShootInterval = 3;  // How often the monster shoots 
    [Export()] public Vector2 StartPosition = Vector2.Zero;
    
    private bool playerInRange = false;
    private bool ableToShoot = true;
    private bool vulnerable = true;

    public override void _Ready()
    {
        shootIntervalTimer = GetNode<Timer>("ShootInterval");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        hurtAudio2D = GetNode<AudioStreamPlayer2D>("HurtAudio2D");
        deathAudio2D = GetNode<AudioStreamPlayer2D>("DeathAudio2D");
        
        ScoreCard = (PackedScene) ResourceLoader.Load("res://Scenes/ScoreCard/ScoreCard.tscn");
        OogShoot = (PackedScene) ResourceLoader.Load("res://Scenes/GreenOog/OogShot.tscn");
        
        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
        shootIntervalTimer.WaitTime = ShootInterval;
        animationPlayer.Play("Fly");
        Position = StartPosition;
        
        hurtTimer = new Timer {WaitTime = 1, OneShot = true};
        AddChild(hurtTimer);
        hurtTimer.Connect("timeout", this, nameof(HurtOver));
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
            case MonsterStates.Dying:
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
        velocity = velocity.MoveToward(Vector2.Zero, (float)Friction * delta);
    }

    public void TakeDamage(int damageAmount)
    {
        if (vulnerable == false)
            return;
        
        Health -= damageAmount;
        
        if (Health <= 0)
        {
            hurtTimer.Stop();
            vulnerable = false;
            deathAudio2D.Play();
            animationPlayer.Play("Dying");
            monsterState = MonsterStates.Dying;
            return;
        }
        
        hurtAudio2D.Play();
        var scoreCard = (ScoreCard) ScoreCard.Instance();
        scoreCard.ScoreText = Health.ToString();
        scoreCard.ScoreColour = global::ScoreCard.ScoreColours.Orange;
        scoreCard.StartPosition = GlobalPosition;
        GetParent().AddChild(scoreCard);

        monsterState = MonsterStates.Hurting;
        animationPlayer.Play("Hurting");
        ableToShoot = false;
        if (hurtTimer.IsStopped())
            hurtTimer.Start();
    }

    private void Died()
    {
        GetTree().CallGroup("Main", "UpdateLevelItems", -1);
        QueueFree();
    }
    
    private void HurtOver()
    {
        ableToShoot = true;
        monsterState = MonsterStates.Flying;
        animationPlayer.Play("Fly");
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
        if (ableToShoot == false)
            return;
        
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
