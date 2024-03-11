using Godot;
using System;

public class LevelManager : Node
{
    [Export]
    public PackedScene playerPrefab;

    [Signal]
    public delegate void player_spawned(Node2D playerNode);

    public override void _Ready()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(playerPrefab == null)
        {
            GD.PrintErr("LevelManager: playerPrefab is null");
            return;
        }
        Node instancedPlayer = playerPrefab.Instance();

        Node spawnTarget = this;
        Node taggedNode = GetTree().Root.FindNode("PlayerSpawn", true, false);
        if(taggedNode != null)
        {
            spawnTarget = taggedNode;
        }
        spawnTarget.AddChild(instancedPlayer);
        EmitSignal("player_spawned", (Node2D)instancedPlayer);
    }
}
