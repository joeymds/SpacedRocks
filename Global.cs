using Godot;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    public int MonsterCount { get; set; }
    public int LevelCount { get; set; }

    public Global()
    {
        GameOver = false;
        gameLevels = new GameLevels();
        NormalBulletTexture = ResourceLoader.Load("res://GFX/Bullets/Bullet-01.png") as Texture;
        PoweredBulletTexture = ResourceLoader.Load("res://GFX/Bullets/Bullet-02.png") as Texture;
        LevelState = LevelStates.Active;
        LevelCount = 1;
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
        RockCount += rockCount;
    }

    public void ResetGame()
    {
        gameLevels.ResetLevels();
        PlayerScore = 0;
        MonsterCount = 0;
        LevelCount = 0;
        PowerUpCountDown = 0;
        RockCount = 0;
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
        if ((gameLevels.NumberOfLevels() - 1) == gameLevels.LevelIndex)
        {
            GetTree().ChangeScene("res://Scenes/EndGame/EndGame.tscn");
        } 
        else
        {
            currentLevel = gameLevels.GetNextLevel();
            LevelCount = currentLevel.LevelNumber;
            LevelState = LevelStates.Active;
            GetTree().ChangeScene("res://Scenes/Main.tscn");
        }
    }

    private void PlayerDeath()
    {
        GetTree().ChangeScene("res://Scenes/EndGame/EndGame.tscn");
    }

}
