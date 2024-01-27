using Godot;
using System;

public class MovingPlatform : Path2D
{
    [Export]
    public float speed = 100;
    [Export]
    public bool loop = false;

    public PathFollow2D followNode;

    private float direction = 1;

    public override void _Ready()
    {
        followNode = GetNode<PathFollow2D>("PathFollow2D");
        if(followNode == null)
        {
            SetProcess(false);
            return;
        }
        followNode.Loop = loop;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        followNode.Offset += delta * speed * direction;
        if(followNode.UnitOffset >= 1 || followNode.UnitOffset <= 0)
        {
            direction *= -1;
        }
    }
}
