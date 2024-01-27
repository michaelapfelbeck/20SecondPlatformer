using Godot;
using System;

public class MovingPlatformRigidBody : RigidBody2D
{
    [Signal]
    public delegate void velocity_update(Vector2 velocity);

    public Vector2 PlatformVelocity { get; private set; }

    private Vector2 prevWorldPosition = Vector2.Zero;
    public override void _Ready()
    {
        prevWorldPosition = GlobalPosition;
    }

    public override void _Process(float delta)
    {
        Vector2 currWorldPosition = GlobalPosition;
        Vector2 positionDelta = currWorldPosition - prevWorldPosition;
        PlatformVelocity = positionDelta / delta;
        EmitSignal("velocity_update", PlatformVelocity);
        prevWorldPosition = currWorldPosition;
    }
}
