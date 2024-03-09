using Godot;
using System;

public class TestTilesLoader : Node2D
{
    [Export]
    public PackedScene geometry;

    public override void _Ready()
    {
        GD.Print("Hai.");
        Node geo = geometry.Instance();
        AddChild(geo);
    }
}
