using Godot;
using System;

public class LevelContainer : Resource
{
    [Export]
    public PackedScene Scene { get; set; }

    [Export]
    public Sprite ScreenShot { get; set; }

    [Export]
    public String Name { get; set; }
}
