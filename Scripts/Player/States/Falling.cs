﻿using Godot;
using System;

class Falling : PlayerState
{
    public override string Description => throw new NotImplementedException(); public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
    public override void Tick()
    {
        GD.Print("Falling");
    }
}