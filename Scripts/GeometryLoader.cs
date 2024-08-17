using Godot;
using System;

public class GeometryLoader : Node2D
{
    [Export]
    public PackedScene geometry;

    public override void _Ready()
    {
        if (geometry != null)
        {
            Load(geometry);
        }
    }

    public void Load(PackedScene geometry)
    {
        Node geo = geometry.Instance();
        AddChild(geo);
    }
}
