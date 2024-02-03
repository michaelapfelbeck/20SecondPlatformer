using Godot;
using System;

public class Platform : Path2D
{
    [Export]
    public bool debug = false;
    [Export]
    public float bounciness = 750;
    [Export]
    public float speed = 150;
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

        SetBounciness();
    }

    private void SetBounciness()
    {
        IPlatform platformNode = GetNode<PlatformRigidBody>("PathFollow2D/OneWayMoving/RigidBody2D");
        if (platformNode != null)
        {
            platformNode.Bounciness = bounciness;
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        followNode.Offset += delta * speed * direction;
        if(followNode.UnitOffset >= 1 || followNode.UnitOffset <= 0)
        {
            direction *= -1;
        }
        if (debug)
        {
            SetBounciness();
        }
    }
}
