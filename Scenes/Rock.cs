using Godot;
using System;
using System.Collections.Generic;
using SpacedRocks.Common;

public class Rock : KinematicBody2D
{
    private Vector2 _screenSize = Vector2.Zero;
    private ScreenWrap _screenWrap;
    private Sprite _rockSprite;
    private double _rotationSpeed;
    private Vector2 _spriteSize = Vector2.Zero;
    private Texture _texture;
    private CollisionShape2D _collision;
    private Particles2D _puff;
    private HitBox _hitBox;
    private CollisionShape2D _collisionHitBox;
    
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

    [Signal]
    public delegate void Death();
    
    public Vector2 _velocity = Vector2.Zero;

    [Export()] public RockSizes _rockSize = RockSizes.Large;
    [Export()] public Vector2 _startPosition = Vector2.Zero;
    [Export()] private Vector2 _initVelocity = Vector2.Zero;
    [Export()] private double _bounce = 1;

    public override void _Ready()
    {
        _puff = GetNode<Particles2D>("Puff");
        _hitBox = GetNode<HitBox>("HitBox");
        
        AddToGroup("Rocks");
        PopulateDictionary();
        _screenSize = GetViewportRect().Size;
        _screenWrap = new ScreenWrap(_screenSize, 8);
        _hitBox.RockSize = _rockSize;
        InitRock(_initVelocity);
    }    

    public void InitRock(Vector2 velocity)
    {
        var rand = new Random();
        _velocity = velocity.Length() > 0 ? velocity : new Vector2(rand.Next(30, 100), 0).Rotated(SpawnPosition.RandomRadian());
        _rotationSpeed = rand.Next(-3, 3);
        
        LoadTexture();
        AttachCollisionShape();
        
        if (_startPosition == Vector2.Zero)
            _startPosition = _screenSize / 2;
        Position = _startPosition;
  
    }
    
    private void LoadTexture()
    {
        _texture = ResourceLoader.Load(_rocks[_rockSize]) as Texture;
        _rockSprite = GetNode<Sprite>("Sprite");
        _rockSprite.Texture = _texture;
        _spriteSize = _texture.GetSize();
    }

    private void AttachCollisionShape()
    {
        var rockWidth = _texture.GetWidth() / 2;
        var rockHeight = _texture.GetHeight() / 2;
        
        _collision = GetNode<CollisionShape2D>("CollisionShape");
        _collisionHitBox = GetNode<CollisionShape2D>("HitBox/CollisionHitBox");
        _collision.Shape = new CircleShape2D {Radius = Math.Min(rockWidth, rockHeight)};
        _collisionHitBox.Shape = new CircleShape2D {Radius = Math.Min(rockWidth - 1, rockHeight - 1)};
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
        _velocity = _velocity.Clamped(500);
        Rotation += (float)_rotationSpeed * delta;
        var collision = MoveAndCollide(_velocity * delta, false, true, false);
        if (collision != null)
        {
            var col = (Node2D) collision.Collider;
            if (col.Name == "player")
                Explode(_rockSize, _velocity, collision.ColliderVelocity);
            else
            {
                _velocity = _velocity.Bounce(collision.Normal) * (float)_bounce;
                _puff.GlobalPosition = collision.Position;
                _puff.Emitting = true;
            }
        }
        Position = _screenWrap.WrappedPosition(Position, _spriteSize);
    }
    
    public void Explode(Rock.RockSizes size, Vector2 velocity, Vector2 hitVelocity)
    {
        ShowExplosion();
        PlayExplosionSound();
        EmitSignal("Death", size, Position, velocity, hitVelocity);
        GetTree().CallGroup("Main", "UpdateRockLevel", -1);
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
