using Godot;
using System;

public class UI : MarginContainer
{
    private Global _global;
    
    public override void _Ready()
    {
        _global = GetTree().Root.GetNode<Global>("Global");
    }
}
