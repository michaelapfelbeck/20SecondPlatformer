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

        Node globalRoot = GetNode<Node>("/root/Global");
        if (globalRoot == null)
        {
            GD.Print("node not found...");
            return;
        }
        GD.Print("!!! found global root!");
        GlobalVars globalVars = globalRoot.GetNode<GlobalVars>("GlobalVars");
        if (globalVars == null)
        {
            GD.Print("node vars not found...");
            return;
        }
        GD.Print($"!!! found global vars! {globalVars.TestMe}");
        //GD.Print($"global var test string: {globalNode.TestMe}");
    }

    private void OnStartButton()
    {
        GD.Print("OnStartButton");
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

    private void OnQuitButton()
    {
        GD.Print("OnQuitButton");
        GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
