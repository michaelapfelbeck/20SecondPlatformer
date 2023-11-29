using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Running : PlayerState
{
    public Running(PlayerController body, AnimatedSprite sprite) : base(body, sprite)
    {
    }

    public override string Description => throw new NotImplementedException();
    public override void OnEnter()
    {
        GD.Print("Enter Running");
        sprite.Play("walk");
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