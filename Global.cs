using Godot;
using System;

public class Global : Node2D
{

    public int ShieldStrength { get; set; }
    public int PlayerScore { get; set; }
    
    public override void _Ready()
    {
        AddToGroup("Global");
        PlayerScore = 0;
        ShieldStrength = 100;
    }

    public void UpdateShield(int updateValue)
    {
        ShieldStrength += updateValue;
        if (ShieldStrength > 100)
            ShieldStrength = 100;
    }

    public void UpdateScore(int updateValue)
    {
        PlayerScore += updateValue;
    }

}
