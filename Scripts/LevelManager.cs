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
        //SpawnPlayerOld();
    }

    private bool spawned = false;
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(spawned )
        {
            return;
        }
        spawned = SpawnPlayer();
    }

    public bool SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            GD.PrintErr("LevelManager: playerPrefab is null");
            return false;
        }
        Node instancedPlayer = playerPrefab.Instance();

        Node spawnTarget = this;
        Node taggedNode = GetTree().Root.FindNode("PlayerSpawn", true, false);
        if (taggedNode != null)
        {
            spawnTarget = taggedNode;
        }
        else
        {
            return false;
        }
        spawnTarget.AddChild(instancedPlayer);
        EmitSignal("player_spawned", (Node2D)instancedPlayer);
        return true;
    }

    public void SpawnPlayerOld()
    {
        if (playerPrefab == null)
        {
            GD.PrintErr("LevelManager: playerPrefab is null");
            return;
        }
        Node instancedPlayer = playerPrefab.Instance();

        Node spawnTarget = this;
        Node taggedNode = GetTree().Root.FindNode("PlayerSpawn", true, false);
        if (taggedNode != null)
        {
            GD.Print("got tagged node");
            spawnTarget = taggedNode;
        }
        else
        {
            GD.Print("no tagged node");
        }
        spawnTarget.AddChild(instancedPlayer);
        EmitSignal("player_spawned", (Node2D)instancedPlayer);
    }
}
