using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class PlayerState: State
{
    protected KinematicBody2D body;
    protected AnimatedSprite sprite;
    protected PlayerBlackboard blackboard;

    protected PlayerState(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite)
    {
        this.body = body;
        this.sprite = sprite;
        this.blackboard = blackboard;
    }


    protected float RunInput(float xVelocity, float delta)
    {
        float velocity = xVelocity;
        if (blackboard.InstantAcceleration)
        {
            velocity = HandleInstantAcc(blackboard.Velocity.x);
        }
        else
        {
            velocity = HandleRegularAcc(blackboard.Velocity.x, delta);
        }
        return velocity;
    }

    private float HandleInstantAcc(float originalVelocity)
    {
        float velocity = originalVelocity;

        velocity = 0;
        bool pressLeft = Input.IsActionPressed("move_left");
        bool pressRight = Input.IsActionPressed("move_right");

        if ((!pressLeft && !pressRight) || (pressLeft && pressRight))
        {
            velocity = 0;
        }
        else if (pressLeft)
        {
            velocity = -blackboard.PlayerMaxSpeed;
        }
        else if (pressRight)
        {
            velocity = blackboard.PlayerMaxSpeed;
        }

        return velocity;
    }
    private float HandleRegularAcc(float originalVelocity, float delta)
    {
        float velocity = originalVelocity;

        bool pressLeft = Input.IsActionPressed("move_left");
        bool pressRight = Input.IsActionPressed("move_right");

        // pressing left and right together cancels each other out
        // if inputing in one direction while moving in the other,
        // slow down even fast by adding Decceleration as well
        if ((!pressLeft && !pressRight) || (pressLeft && pressRight))
        {
            if (velocity < 0)
            {
                velocity = Math.Min(velocity + blackboard.Decceleration * delta, 0);
            }
            else if (velocity > 0)
            {
                velocity = Math.Max(velocity - blackboard.Decceleration * delta, 0);
            }
        }
        else if (pressLeft)
        {
            float acc = blackboard.Acceleration;
            if(velocity > 0)
            {
                acc += blackboard.Decceleration;
            }
            velocity -= acc * delta;
        }
        else if (pressRight)
        {
            float acc = blackboard.Acceleration;
            if (velocity < 0)
            {
                acc += blackboard.Decceleration;
            }
            velocity += acc * delta;
        }

        velocity = Clamp(velocity, -blackboard.PlayerMaxSpeed, blackboard.PlayerMaxSpeed);

        return velocity;
    }

    protected float JumpFromGround()
    {
        float result = 0;
        if (Input.IsActionJustPressed("jump") && body.IsOnFloor())
        {
            result = blackboard.JumpForce;
        }
        return result;
    }
    protected float DoubleJump()
    {
        float result = 0;
        if (blackboard.DoubleJump && !blackboard.DoubleJumped && Input.IsActionJustPressed("jump"))
        {
            result = blackboard.JumpForce;
            blackboard.DoubleJumped = true;
        }

        return result;
    }

    protected float ApplyGravity(float value, float gravity, float delta)
    {
        float velocity = value;
        velocity += gravity * delta;
        velocity = Math.Min(velocity, blackboard.TerminalVelocity);
        return velocity;
    }

    protected float Clamp(float value, float low, float high)
    {
        return Math.Max(Math.Min(value, high), low);
    }

    protected void SetFacing()
    {

        if (blackboard.Velocity.x < 0)
        {
            sprite.FlipH = true;
        }
        else if (blackboard.Velocity.x > 0)
        {
            sprite.FlipH = false;
        }
    }
}
