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
        velocity.x = 0;
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x -= blackboard.PlayerMaxSpeed;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += blackboard.PlayerMaxSpeed;
        }

        if (Input.IsActionJustReleased("jump"))
        {
            jumpReleased = true;
        }

        velocity.y += Gravity * delta;

        blackboard.Velocity = velocity;
        SetFacing();
    }
}