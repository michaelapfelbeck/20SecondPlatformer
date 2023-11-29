using Godot;
using System;

class Falling : PlayerState
{
    public Falling(PlayerController body, AnimatedSprite sprite) : base(body, sprite)
    {
    }

    public override string Description => throw new NotImplementedException(); 
    public override void OnEnter()
    {
        GD.Print("Enter Falling");
        sprite.Play("jump");
    }
    public override void OnExit()
    {
    }
    public override void Tick()
    {
        if (body.velocity.x < 0)
        {
            sprite.FlipH = true;
        }
        else if (body.velocity.x > 0)
        {
            sprite.FlipH = false;
        }
        //GD.Print("Falling");
    }
}