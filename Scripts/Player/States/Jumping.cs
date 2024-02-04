using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Jumping : PlayerState
{
    private bool jumpReleased = false;

    private float Gravity { get { return jumpReleased ? blackboard.FallGravity : blackboard.Gravity; } }

    public Jumping(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }
    public override string Description => "Jumping"; 
    public override void OnEnter()
    {
        blackboard.CoyoteBuffer.Clear();
        jumpReleased = blackboard.JumpBuffer.Released;
        sprite.Play("jump");
        sprite.SpeedScale = 1f;
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        bool cutVelocity = false;

        if (blackboard.JumpBuffer.Released)
        {
            if (blackboard.VelocityCut && !jumpReleased && blackboard.Velocity.y < 0)
            {
                cutVelocity = true;
            }
            jumpReleased = true;
        }

        if(jumpReleased && blackboard.DoubleJump)
        {
            float result = DoubleJump();
            if(result != 0)
            {
                velocity.y = -result;
                jumpReleased = false;
            }
        }

        velocity.y = ApplyGravity(velocity.y, Gravity, delta);
        if (cutVelocity)
        {
            velocity.y *= 0.3f;
        }

        blackboard.Velocity = velocity;
        SetFacing();
    }
}