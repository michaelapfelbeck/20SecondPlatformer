using Godot;
using System;

public class TitleScreen : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Title screen ready");
    }

    private void OnStartButton()
    {
        GD.Print("OnStartButton");
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
