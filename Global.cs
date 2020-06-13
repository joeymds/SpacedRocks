using Godot;
using System.Collections.Generic;
using SpacedRocks.Common;

public class Global : Node2D
{
    public enum LevelStates
    {
        Active, Complete, Failed        
    }

    private int _level = 1;
    private List<Level> _levels = new List<Level>();
    private Level _currentLevel;

    public int Shield { get; set; }
    public int PlayerScore { get; set; }
    public int RockCount { get; set; }

    public LevelStates LevelState { get; set; }

    public Global()
    {
        createLevels();
        LevelState = LevelStates.Active;
    }
    
    public Level Level => _currentLevel;

    public override void _Ready()
    {
        AddToGroup("Global");
        PlayerScore = 0;
        _currentLevel = _levels[0];
    }

    public void UpdateScore(int updateValue)
    {
        PlayerScore += updateValue;
    }

    public void UpdateRockCount(int rockCount)
    {
        RockCount = rockCount;
    }

    private void FinishedLevel()
    {
        LevelState = LevelStates.Complete;
    }

    private void AdvanceLevel()
    {
        _level++;
        _currentLevel = _levels[_level - 1];
        LevelState = LevelStates.Active;
    }
    
    
    
    private void createLevels()
    {
        _levels.Add(new Level()
        {
            LevelNumber = 1, 
            LevelName = "Level01",
            NumberOfRocks = 1
        });
        _levels.Add(new Level()
        {
            LevelNumber = 2, 
            LevelName = "Level02",
            NumberOfRocks = 2
        });
        _levels.Add(new Level()
        {
            LevelNumber = 3, 
            LevelName = "Level03",
            NumberOfRocks = 3
        });
        _levels.Add(new Level()
        {
            LevelNumber = 4, 
            LevelName = "Level04",
            NumberOfRocks = 4
        });
        _levels.Add(new Level()
        {
            LevelNumber = 5, 
            LevelName = "Level05",
            NumberOfRocks = 5
        });
        _levels.Add(new Level()
        {
            LevelNumber = 6, 
            LevelName = "Level06",
            NumberOfRocks = 6
        });
    }

}
