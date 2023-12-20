using Godot;
using System;

class Falling : PlayerState
{
    public Falling(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }

    public override string Description => "Falling"; 
    public override void OnEnter()
    {
        sprite.Play("jump");
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        float result = DoubleJump();
        if(result != 0)
        {
            velocity.y = -result;
        }

        velocity.y = ApplyGravity(velocity.y, blackboard.FallGravity, delta) ;

        blackboard.Velocity = velocity;
        SetFacing();
    }
}