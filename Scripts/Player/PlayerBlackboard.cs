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
    bool VelocityCut { get; }
    float JumpForce { get; }
    float Gravity { get; }
    float FallGravity { get; }
    float TerminalVelocity { get; }
    float WallSlideVelocity { get; }
    bool IsWallSlide { get; }
    Direction WallSlideDirection { get; }

    bool InstantAcceleration { get; }
    float PlayerMaxSpeed { get; }
    float Acceleration { get; }
    float Decceleration { get; }
    BufferButton JumpBuffer { get; }
    BoolBuffer CoyoteBuffer { get; }
}
