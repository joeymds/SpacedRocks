using Godot;
using System.Collections.Generic;

public class Main : Node
{
    private Global _global;
    private PackedScene spaceRockScene;
    private Node _rockSpawnLocations;
    private Node _rockContainer;
    private int _totalNumberOfRocks;
    private int _levelRockCount;
    private CountDown _countDown;
    private AudioStreamPlayer _gameTrack;
    private Dictionary<Rock.RockSizes, Rock.RockSizes> _breakPattern = new Dictionary<Rock.RockSizes, Rock.RockSizes>();
    
    public override void _Ready()
    {
        AddToGroup("Main");
        PopulateDictionary();
        
        _global = GetTree().Root.GetNode<Global>("Global");
        _rockSpawnLocations = GetNode<Node>("RockSpawnLocations");
        _rockContainer = GetNode<Node>("RockContainer");
        _countDown = GetNode<CountDown>("CountDown");
        _gameTrack = GetNode<AudioStreamPlayer>("GameTrack");
        
        spaceRockScene = (PackedScene) ResourceLoader.Load("res://Scenes/Rock.tscn");
        _totalNumberOfRocks = _global.Level.NumberOfRocks;
        _levelRockCount = _global.Level.NumberOfRocks;
        _countDown.Visible = false;
        
        for (var i = 0; i < _totalNumberOfRocks; i++)
            SpawnRock(Rock.RockSizes.Large, _rockSpawnLocations.GetChild<Position2D>(i).Position, Vector2.Zero);
        
        _gameTrack.Play();    
        
    }

    private void PopulateDictionary()
    {
        _breakPattern.Add(Rock.RockSizes.Large, Rock.RockSizes.Medium);
        _breakPattern.Add(Rock.RockSizes.Medium, Rock.RockSizes.Small);
        _breakPattern.Add(Rock.RockSizes.Small, Rock.RockSizes.Tiny);
        _breakPattern.Add(Rock.RockSizes.Tiny, Rock.RockSizes.Dead);
    }

    private void SpawnRock(Rock.RockSizes rockSize, Vector2 position, Vector2 velocity)
    {
        var spaceRock = (Rock) spaceRockScene.Instance();
        _rockContainer.AddChild(spaceRock);
        spaceRock._rockSize = rockSize;
        spaceRock._startPosition = position;
        spaceRock.Connect("Death", this, nameof(ExplodeRock));
        spaceRock.InitRock(velocity);
    }

    private void ExplodeRock(Rock.RockSizes size, Vector2 position, Vector2 velocity, Vector2 hitVelocity)
    {
        var newSize = _breakPattern[size];
        var offsets = new List<int> {-1, 1};
        if (newSize == Rock.RockSizes.Dead) return;
        foreach (var offset in offsets)
        {
            UpdateRockLevel(1);
            var newPosition = position + hitVelocity.Tangent().Clamped(25) * offset;
            var newVelocity = (velocity + hitVelocity.Tangent()) * offset;
            SpawnRock(newSize, newPosition, newVelocity);
        }
    }

    public void UpdateRockLevel(int rocks)
    {
        _levelRockCount += rocks;
        if (_levelRockCount <= 0)
        {
            GetTree().CallGroup("Global", "FinishedLevel");
            _countDown.Visible = true;
            _countDown.StartCountDown();
        }
    }

    private void CountDownComplete()
    {
        QueueFree();
        GetTree().CallGroup("Global", "AdvanceLevel");
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
