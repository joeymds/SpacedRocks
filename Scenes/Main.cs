using Godot;
using System.Collections.Generic;

public class Main : Node2D
{
    private PackedScene spaceRockScene;
    private Node _rockSpawnLocations;
    private Node _rockContainer;
    private Dictionary<Rock.RockSizes, Rock.RockSizes> _breakPattern = new Dictionary<Rock.RockSizes, Rock.RockSizes>();
    
    public override void _Ready()
    {
        PopulateDictionary();
        _rockSpawnLocations = GetNode<Node>("RockSpawnLocations");
        _rockContainer = GetNode<Node>("RockContainer");
        spaceRockScene = (PackedScene) ResourceLoader.Load("res://Scenes/Rock.tscn");
        
        for (var i = 0; i < 2; i++)
            SpawnRock(Rock.RockSizes.Large, _rockSpawnLocations.GetChild<Position2D>(i).Position, Vector2.Zero);
        
    }

    private void PopulateDictionary()
    {
        _breakPattern.Add(Rock.RockSizes.Large, Rock.RockSizes.Medium);
        _breakPattern.Add(Rock.RockSizes.Medium, Rock.RockSizes.Small);
        _breakPattern.Add(Rock.RockSizes.Small, Rock.RockSizes.Tiny);
        _breakPattern.Add(Rock.RockSizes.Tiny, Rock.RockSizes.Dead);
    }
    
    public void SpawnRock(Rock.RockSizes rockSize, Vector2 position, Vector2 velocity)
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
            var newPosition = position + hitVelocity.Tangent().Clamped(25) * offset;
            var newVelocity = (velocity + hitVelocity.Tangent()) * offset;
            SpawnRock(newSize, newPosition, newVelocity);
        }
    }
}
