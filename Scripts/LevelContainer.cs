using Godot;
using System;

public class LevelContainer : Resource
{
    [Export]
    public PackedScene Scene { get; set; }

    [Export]
    public Texture Icon { get; set; }

    [Export]
    public String Name { get; set; }
}
