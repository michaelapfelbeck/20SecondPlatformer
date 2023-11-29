using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Jumping : PlayerState
{
    public Jumping(PlayerController body, AnimatedSprite sprite) : base(body, sprite)
    {
    }
    public override string Description => throw new NotImplementedException(); 
    public override void OnEnter()
    {
        GD.Print("Enter Jumping");
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
    }
}