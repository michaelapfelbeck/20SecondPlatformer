using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlatform
{
    Vector2 PlatformVelocity { get; }
    float Bounciness { get; set; }
}
