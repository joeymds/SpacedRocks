using Godot;
using System;
using System.Collections.Generic;
using SpacedRocks.Common;

public class Rock : KinematicBody2D
{
    private Vector2 screenSize = Vector2.Zero;
    private Vector2 spriteSize = Vector2.Zero;
    private ScreenWrap screenWrap;
    private Sprite rockSprite;
    private Texture texture;
    private CollisionShape2D collision;
    private Particles2D puff;
    private HitBox hitBox;
    private CollisionShape2D collisionHitBox;
    private PackedScene ScoreCard;
    private EntityScores entityScores;

    private double _rotationSpeed;
    
    public enum RockSizes
    {
        Tiny,
        Small,
        Medium,
        Large,
        Dead
    };
    
    public enum RockDamage
    {
        Tiny = 5, Small = 8, Medium = 13, Large = 24
    }
    
    public readonly Dictionary<RockSizes, string> _rocks = new Dictionary<RockSizes, string>();

    public Vector2 rockVelocity = Vector2.Zero;

    [Export()] public RockSizes rockSize = RockSizes.Large;
    [Export()] public Vector2 _startPosition = Vector2.Zero;
    [Export()] private Vector2 _initVelocity = Vector2.Zero;
    [Export()] private double _bounce = 1;

    public override void _Ready()
    {
        entityScores = new EntityScores();
        
        puff = GetNode<Particles2D>("Puff");
        hitBox = GetNode<HitBox>("HitBox");

        ScoreCard = (PackedScene) ResourceLoader.Load("res://Scenes/ScoreCard/ScoreCard.tscn");
        
        AddToGroup("Rocks");
        PopulateDictionary();
        screenSize = GetViewportRect().Size;
        screenWrap = new ScreenWrap(screenSize, 8);
        hitBox.RockSize = rockSize;
        InitRock(_initVelocity);
    }    

    public void InitRock(Vector2 velocity)
    {
        var rand = new Random();
        rockVelocity = velocity.Length() > 0 ? velocity : new Vector2(rand.Next(30, 100), 0).Rotated(SpawnPosition.RandomRadian());
        _rotationSpeed = rand.Next(-3, 3);
        
        LoadTexture();
        AttachCollisionShape();
        
        if (_startPosition == Vector2.Zero)
            _startPosition = screenSize / 2;
        Position = _startPosition;
  
    }
    
    private void LoadTexture()
    {
        texture = ResourceLoader.Load(_rocks[rockSize]) as Texture;
        rockSprite = GetNode<Sprite>("Sprite");
        rockSprite.Texture = texture;
        spriteSize = texture.GetSize();
    }

    private void AttachCollisionShape()
    {
        var rockWidth = texture.GetWidth() / 2;
        var rockHeight = texture.GetHeight() / 2;
        
        collision = GetNode<CollisionShape2D>("CollisionShape");
        collisionHitBox = GetNode<CollisionShape2D>("HitBox/CollisionHitBox");
        collision.Shape = new CircleShape2D {Radius = Math.Min(rockWidth, rockHeight)};
        collisionHitBox.Shape = new CircleShape2D {Radius = Math.Min(rockWidth - 1, rockHeight - 1)};
    }
    
    private void PopulateDictionary()
    {
        _rocks.Add(RockSizes.Large, GetRockSprite(RockSizes.Large));
        _rocks.Add(RockSizes.Medium, GetRockSprite(RockSizes.Medium));
        _rocks.Add(RockSizes.Small, GetRockSprite(RockSizes.Small));
        _rocks.Add(RockSizes.Tiny, GetRockSprite(RockSizes.Tiny));
        _rocks.Add(RockSizes.Dead, "");
    }

    private static string GetRockSprite(RockSizes size)
    {
        Random rndRockImage = new Random();
        var imageIndex = rndRockImage.Next(1, 3);
        var rockImageSprite = $"res://GFX/Rocks/large-rock-{imageIndex}.png";
        switch (size)
        {
            case RockSizes.Large:
                rockImageSprite = $"res://GFX/Rocks/large-rock-{imageIndex}.png";
                break;
            case RockSizes.Medium:
                rockImageSprite = $"res://GFX/Rocks/medium-rock-{imageIndex}.png";
                break;
            case RockSizes.Small:
                rockImageSprite = $"res://GFX/Rocks/small-rock-{imageIndex}.png";
                break;
            case RockSizes.Tiny:
                rockImageSprite = $"res://GFX/Rocks/tiny-rock-{imageIndex}.png";
                break;
        }
        return rockImageSprite;
    }
    
    public override void _PhysicsProcess(float delta)
    {
        rockVelocity = rockVelocity.Clamped(500);
        Rotation += (float)_rotationSpeed * delta;
        var collision = MoveAndCollide(rockVelocity * delta, false, true, false);
        if (collision != null)
        {
            rockVelocity = rockVelocity.Bounce(collision.Normal) * (float)_bounce;
            puff.GlobalPosition = collision.Position;
            puff.Emitting = true;
        }
        Position = screenWrap.WrappedPosition(Position, spriteSize);
    }
    
    public void Explode(Rock.RockSizes size, Vector2 velocity, Vector2 hitVelocity)
    {
        var scoreCard = (ScoreCard) ScoreCard.Instance();
        scoreCard.ScoreText = entityScores.getRockScore(size).ToString();
        scoreCard.StartPosition = GlobalPosition;
        GetParent().AddChild(scoreCard);
        
        GetTree().CallGroup("Main", "ExplodeRock", rockSize, Position, velocity, hitVelocity);
        ShowExplosion();
        PlayExplosionSound();
        QueueFree();
    }

    private void ShowExplosion()
    {
        var explosionScene = (PackedScene)ResourceLoader.Load("res://Scenes/Explosion.tscn");
        var explosion = (Node2D)explosionScene.Instance();
        GetTree().Root.AddChild(explosion);
        explosion.GlobalPosition = Position;
    }
    
    private void PlayExplosionSound()
    {
        var explosionScene = (PackedScene)ResourceLoader.Load("res://Scenes/RockExplosion.tscn");
        var explosion = (Node2D)explosionScene.Instance();
        GetTree().Root.AddChild(explosion);
        explosion.GlobalPosition = Position;
    }

}
