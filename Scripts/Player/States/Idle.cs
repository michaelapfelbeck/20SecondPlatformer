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
        blackboard.DoubleJumped = false;
        sprite.Play("idle");
        sprite.SpeedScale = 1f;
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        velocity.y = ApplyGravity(velocity.y, blackboard.Gravity, delta);

        velocity.y -= JumpFromGround();

        blackboard.Velocity = velocity;
        SetFacing();
    }
}