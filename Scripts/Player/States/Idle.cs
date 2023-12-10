using Godot;
using System;

class Idle : PlayerState
{
    public Idle(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }

    public override string Description => "Idle"; 
    public override void OnEnter()
    {
        sprite.Play("front");
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        velocity.y += blackboard.Gravity * delta;

        if (Input.IsActionPressed("jump") && body.IsOnFloor())
        {
            velocity.y -= blackboard.JumpForce;
        }
        blackboard.Velocity = velocity;
        SetFacing();
    }
}