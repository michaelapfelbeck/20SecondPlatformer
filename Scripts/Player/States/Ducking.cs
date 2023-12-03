using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Ducking : PlayerState
{
    public Ducking(KinematicBody2D body, PlayerBlackboard blackboard, AnimatedSprite sprite) : base(body, blackboard, sprite)
    {
    }
    public override string Description => "Ducking";

    public override void OnEnter()
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override void Tick(float delta)
    {
        throw new NotImplementedException();
    }
}
