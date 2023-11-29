using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class PlayerState: State
{
    protected PlayerController body;
    protected AnimatedSprite sprite;

    protected PlayerState(PlayerController body, AnimatedSprite sprite)
    {
        this.body = body;
        this.sprite = sprite;
    }
}
