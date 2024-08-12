using Godot;
using System;

public class TestTilesLoader : Node2D
{
    [Export]
    public PackedScene geometry;

    public override void _Ready()
    {
        Node geo = geometry.Instance();
        AddChild(geo);
    }
}
