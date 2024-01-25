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
    }

    public override void OnExit()
    {
    }

    public override void Tick(float delta)
    {
        SetWallFacing();
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
}
