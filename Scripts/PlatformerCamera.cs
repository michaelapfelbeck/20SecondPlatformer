using Godot;
using System;

public class PlatformerCamera : Camera2D
{
    [Export]
    private NodePath player;
 
    private Node2D followTarget;

    public override void _Ready()
    {
        GD.Print("hai.");
        SetupFollowPlayer();
    }

    private void SetupFollowPlayer()
    {
        Node result = GetNode(player);
        if (result == null)
        {
            GD.Print("Player not found");
            return;
        }
        followTarget = (Node2D)result;
    }

    public override void _Process(float delta)
    {
        if(followTarget != null)
        {
            this.Position = followTarget.Position;
        }
    }
}
