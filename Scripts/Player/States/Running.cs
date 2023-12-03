using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Running : PlayerState
{
    public Running(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }

    public override string Description => "Running";
    public override void OnEnter()
    {
        sprite.Play("walk");
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

        velocity.y += blackboard.Gravity * delta;

        if (Input.IsActionPressed("jump") && body.IsOnFloor())
        {
            velocity.y -= blackboard.JumpForce;
        }
        blackboard.Velocity = velocity;
        SetFacing();
    }
}