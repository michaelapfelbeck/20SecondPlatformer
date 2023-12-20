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
        jumpReleased = false;
        sprite.Play("jump");
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        if (Input.IsActionJustReleased("jump"))
        {
            jumpReleased = true;
        }

        if(jumpReleased && blackboard.DoubleJump)
        {
            float result = DoubleJump();
            if(result != 0)
            {
                velocity.y = -result;
            }
        }

        velocity.y = ApplyGravity(velocity.y, Gravity, delta);

        blackboard.Velocity = velocity;
        SetFacing();
    }
}