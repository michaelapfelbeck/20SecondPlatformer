using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WallSlide : PlayerState
{
    public override string Description => "Wall Slide";
    public WallSlide(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }

    public override void OnEnter()
    {
        blackboard.DoubleJumped = false;
        sprite.Play("wallJump");
        sprite.SpeedScale = 1f;
        SetWallFacing();
    }

    public override void OnExit()
    {
    }

    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;
        velocity.y = ApplyGravity(velocity.y, blackboard.Gravity, delta);
        velocity += JumpFromWall();
        blackboard.Velocity = velocity;
    }

    protected void SetWallFacing()
    {

        if (blackboard.WallSlideDirection == Direction.Right)
        {
            sprite.FlipH = false;
        }
        else
        {
            sprite.FlipH = true;
        }
    }
    private Vector2 JumpFromWall()
    {
        Vector2 velocity = Vector2.Zero;
        
        if (blackboard.JumpBuffer.JustPressed)
        {
            GD.Print("jumped from wall");
            velocity.y -= blackboard.JumpForce;
            blackboard.JumpBuffer.Consume();
            if(blackboard.WallSlideDirection == Direction.Left)
            {
                velocity.x += blackboard.PlayerMaxSpeed;
            } else
            {
                velocity.x -= blackboard.PlayerMaxSpeed;
            }
        }
        return velocity;
    }
}
