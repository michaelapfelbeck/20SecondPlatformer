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

        float movementInput = Input.GetAxis("move_left", "move_right");

        if (movementInput == 0)
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
        else
        {
            float acc = blackboard.Acceleration;
            // if the movement input is in the opposite direction of player traffic
            // use decceleration to slow down more quickly
            if ((velocity > 0 && movementInput < 0) || (velocity < 0 && movementInput > 0))
            {
                acc += blackboard.Decceleration;
            }
            velocity += acc * delta * movementInput;
        }

        velocity = Clamp(velocity, -blackboard.PlayerMaxSpeed, blackboard.PlayerMaxSpeed);

        return velocity;
    }

    protected float JumpFromGround()
    {
        float result = 0;
        if (blackboard.JumpBuffer.JustPressed && blackboard.CoyoteBuffer.Value)
        {
            result = blackboard.JumpForce;
            blackboard.JumpBuffer.Consume();
        }
        return result;
    }
    protected float DoubleJump()
    {
        float result = 0;
        if (blackboard.DoubleJump && !blackboard.DoubleJumped && blackboard.JumpBuffer.JustPressed)
        {
            result = blackboard.JumpForce;
            blackboard.JumpBuffer.Consume();
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
