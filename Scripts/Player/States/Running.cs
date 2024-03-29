﻿using Godot;
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
        blackboard.DoubleJumped = false;
        sprite.Play("run");
        sprite.SpeedScale = 3f;
    }
    public override void OnExit()
    {
    }
    public override void Tick(float delta)
    {
        Vector2 velocity = blackboard.Velocity;

        velocity.x = RunInput(velocity.x, delta);

        velocity.y = ApplyGravity(velocity.y, blackboard.Gravity, delta);

        velocity.y -= JumpFromGround();

        blackboard.Velocity = velocity;
        SetFacing();
    }
}