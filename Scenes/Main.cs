using Godot;
using System.Collections.Generic;
using SpacedRocks.Common;

public class Main : Node
{
    private Global global;
    private PackedScene spaceRockScene;
    private PackedScene powerUpScene;
    private Node rockSpawnLocations;
    private Node powerUpSpawnLocations;
    private Node assetContainer;
    
    private int totalNumberOfRocks;
    private int levelRockCount;
    private int levelPowerUpCount;

    private SpawnPosition spawnPosition;
    private CountDown countDown;
    private AudioStreamPlayer gameTrack;
    private Camera2D camera;
    private Dictionary<Rock.RockSizes, Rock.RockSizes> breakPattern = new Dictionary<Rock.RockSizes, Rock.RockSizes>();
    
    public override void _Ready()
    {
        AddToGroup("Main");
        PopulateDictionary();
        
        global = GetTree().Root.GetNode<Global>("Global");
        rockSpawnLocations = GetNode<Node>("RockSpawnLocations");
        powerUpSpawnLocations = GetNode<Node>("PowerUpSpawnLocations");
        assetContainer = GetNode<Node>("AssetContainer");
        countDown = GetNode<CountDown>("CountDown");
        gameTrack = GetNode<AudioStreamPlayer>("GameTrack");
        camera = GetNode<Camera2D>("Camera2D");
        
        spawnPosition = new SpawnPosition(11);
        spaceRockScene = (PackedScene) ResourceLoader.Load("res://Scenes/Rock.tscn");
        powerUpScene = (PackedScene) ResourceLoader.Load("res://Scenes/PowerUp/PowerUp.tscn");
        
        totalNumberOfRocks = global.Level.NumberOfRocks;
        levelRockCount = global.Level.NumberOfRocks;
        levelPowerUpCount = global.Level.NumberOfPowerUps;

        for (var i = 0; i < totalNumberOfRocks; i++)
        {
            var positionIndex = spawnPosition.GetNextPositionIndex();
            SpawnRock(Rock.RockSizes.Large, rockSpawnLocations.GetChild<Position2D>(positionIndex).Position, Vector2.Zero);
        }

        for (var i = 0; i < levelPowerUpCount; i++)
            SpawnPowerUp(i);

        gameTrack.Play();    
        
    }

    private void PopulateDictionary()
    {
        breakPattern.Add(Rock.RockSizes.Large, Rock.RockSizes.Medium);
        breakPattern.Add(Rock.RockSizes.Medium, Rock.RockSizes.Small);
        breakPattern.Add(Rock.RockSizes.Small, Rock.RockSizes.Tiny);
        breakPattern.Add(Rock.RockSizes.Tiny, Rock.RockSizes.Dead);
    }

    private void SpawnPowerUp(int positionIndex)
    {
        var powerUp = (PowerUp) powerUpScene.Instance();
        assetContainer.AddChild(powerUp);
        powerUp.startPosition = powerUpSpawnLocations.GetChild<Position2D>(positionIndex).Position;
    }
    
    private void SpawnRock(Rock.RockSizes rockSize, Vector2 position, Vector2 velocity)
    {
        var spaceRock = (Rock) spaceRockScene.Instance();
        assetContainer.AddChild(spaceRock);
        spaceRock.rockSize = rockSize;
        spaceRock._startPosition = position;
        spaceRock.InitRock(velocity);
    }

    private void ExplodeRock(Rock.RockSizes size, Vector2 position, Vector2 velocity, Vector2 hitVelocity)
    {
        var newSize = breakPattern[size];
        var offsets = new List<int> {-1, 1};
        
        if (newSize == Rock.RockSizes.Dead)
        {
            UpdateRockLevel(-1);
            return;
        }
        
        foreach (var offset in offsets)
        {
            UpdateRockLevel(1);
            var newPosition = position + hitVelocity.Tangent().Clamped(25) * offset;
            var newVelocity = (velocity + hitVelocity.Tangent()) * offset;
            SpawnRock(newSize, newPosition, newVelocity);
        }
        UpdateRockLevel(-1);
    }

    private void UpdateRockLevel(int rocks)
    {
        levelRockCount += rocks;
        if (levelRockCount > 0) return;
        countDown.StartCountDown();
    }

    private void CountDownComplete()
    {
        QueueFree();
        GetTree().CallGroup("Global", "AdvanceLevel");
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
