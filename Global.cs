using Godot;
using System.Collections.Generic;
using SpacedRocks.Common;

public class Global : Node2D
{
    public enum LevelStates
    {
        Active, Complete, Failed        
    }
    
    private List<Level> levels = new List<Level>();
    private Level currentLevel;

    public Texture NormalBulletTexture;
    public Texture PoweredBulletTexture;

    private readonly GameLevels gameLevels;

    public int Shield { get; set; }     
    public int PlayerScore { get; set; }
    public int RockCount { get; set; }
    public int PowerUpCountDown { get; set; }
    public bool GameOver { get; set; }
    public LevelStates LevelState { get; set; }

    public Global()
    {
        GameOver = false;
        gameLevels = new GameLevels();
        NormalBulletTexture = ResourceLoader.Load("res://GFX/Bullets/Bullet-01.png") as Texture;
        PoweredBulletTexture = ResourceLoader.Load("res://GFX/Bullets/Bullet-02.png") as Texture;
        LevelState = LevelStates.Active;
    }
    
    public Level Level => currentLevel;

    public override void _Ready()
    {
        AddToGroup("Global");
        ResetGame();
    }

    public void RockHit(int scoreValue)
    {
        PlayerScore += scoreValue;
    }

    public void UpdateRockCount(int rockCount)
    {
        RockCount = rockCount;
    }

    public void ResetGame()
    {
        gameLevels.ResetLevels();
        PlayerScore = 0;
        PowerUpCountDown = 0;
        GameOver = false;
        currentLevel = gameLevels.GetNextLevel();
    }

    public void SetPowerUpValue(int value)
    {
        PowerUpCountDown = value;
    }

    private void UpdateShield(int shieldValue)
    {
        Shield = shieldValue;
    }
    
    private void FinishedLevel()
    {
        LevelState = LevelStates.Complete;
    }

    private void AdvanceLevel()
    {
        PowerUpCountDown = 0;
        currentLevel = gameLevels.GetNextLevel();
        LevelState = LevelStates.Active;
    }

}
