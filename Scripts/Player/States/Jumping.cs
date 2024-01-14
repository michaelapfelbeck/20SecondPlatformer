﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Jumping : PlayerState
{
    private bool jumpReleased = false;

    private float Gravity { get { return jumpReleased ? blackboard.FallGravity : blackboard.Gravity; } }

    private bool velocityCut = false;

    public Jumping(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite, bool velocityCut) : base(body, blackboard, sprite)
    {
        this.velocityCut = velocityCut;
    }
    public override string Description => "Jumping"; 
    public override void OnEnter()
    {
        blackboard.CoyoteBuffer.Clear();
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

        bool cutVelocity = false;

        if (blackboard.JumpBuffer.Released)
        {
            if (velocityCut && !jumpReleased && blackboard.Velocity.y < 0)
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