using Godot;
using System;

public class PlatformerCamera : Camera2D
{
    [Export]
    private bool lookahead = true;

    [Export]
    private float lookaheadSpeed = 10f;

    [Export]
    private Vector2 lookAheadMax = Vector2.Zero;

    [Export]
    private NodePath player;
 
    private PlayerBlackboard blackboard;

    public override void _Ready()
    {
        GetPlayerBlackboard();
    }

    private void GetPlayerBlackboard()
    {
        Node result = GetNode(player);
        if (result == null)
        {
            GD.PrintErr("Player not found");
            lookahead = false;
            return;
        }
        blackboard = (PlayerBlackboard)result;
    }

    public override void _Process(float delta)
    {
        // adjust camera offset based on player velocity to show more of the game space
        // in the direction the player is moving
        if (lookahead)
        {
            
            float ratioH = blackboard.Velocity.x / blackboard.PlayerMaxSpeed;
            float ratioV = blackboard.Velocity.y / blackboard.PlayerMaxSpeed;
            this.OffsetH = Mathf.Lerp(this.OffsetH, ratioH * lookAheadMax.x, lookaheadSpeed * delta);
            this.OffsetV = Mathf.Lerp(this.OffsetV, ratioV * lookAheadMax.y, lookaheadSpeed * delta);
            // GD.Print(String.Format("Lookahead: {0}", this.OffsetH));
        }
    }
}
