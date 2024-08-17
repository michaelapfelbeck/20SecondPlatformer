using Godot;
using System;

public class LevelLoader : Node
{
    [Export]
    public PackedScene levelTemplate;
    //[Export]
    //public NodePath levelTemplate;
    //[Export]
    //public String levelPath;

    [Export]
    public PackedScene testGeometry;

    public void LoadTest()
    {
        GD.Print("LevelLoader LoadTest");
        GetTree().ChangeSceneTo(levelTemplate);
        //GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
