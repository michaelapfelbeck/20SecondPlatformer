﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface PlayerBlackboard
{
    Vector2 Velocity { get; set; }
    float JumpForce { get; }
    float Gravity { get; }
    float FallGravity { get; }
    float PlayerMaxSpeed { get; }
}
