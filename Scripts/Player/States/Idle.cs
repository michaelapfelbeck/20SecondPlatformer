using Godot;
using System;

class Idle : PlayerState
{
    public Idle(PlayerController body, AnimatedSprite sprite) : base(body, sprite)
    {
    }

    public override string Description => throw new NotImplementedException(); 
    public override void OnEnter()
    {
        GD.Print("Enter Idle");
        sprite.Play("front");
    }
    public override void OnExit()
    {
    }
    public override void Tick()
    {
        //GD.Print("Idle");
    }
}