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
