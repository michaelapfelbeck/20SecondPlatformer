using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface PlayerBlackboard
{
    Vector2 Velocity { get; set; }
    bool DoubleJump { get; }
    bool DoubleJumped { get; set; }
    float JumpForce { get; }
    float Gravity { get; }
    float FallGravity { get; }
    float TerminalVelocity { get; }

    bool InstantAcceleration { get; }
    float PlayerMaxSpeed { get; }
    float Acceleration { get; }
    float Decceleration { get; }
    BufferButton JumpBuffer { get; }
    BoolBuffer CoyoteBuffer { get; }
}
