using Godot;
using System;

public class TitleScreen : Node
{
    [Export]
    public NodePath defaultFocus;

    public override void _Ready()
    {
        //GD.Print("Title screen ready");

        Node globalRoot = GetNode<Node>("/root/Global");
        if (globalRoot == null)
        {
            //GD.Print("node not found...");
            return;
        }
        //GD.Print("!!! found global root!");
        GlobalVars globalVars = globalRoot.GetNode<GlobalVars>("GlobalVars");
        if (globalVars == null)
        {
            //GD.Print("node vars not found...");
            return;
        }
        //GD.Print($"!!! found global vars! {globalVars.TestMe}");
        //GD.Print($"global var test string: {globalNode.TestMe}");
        if (defaultFocus != null) {
            Control focusNode = GetNode<Control>(defaultFocus);
            focusNode.GrabFocus();
        }
    }

    private void OnStartButton()
    {
        GD.Print("OnStartButton");
        //Node globalRoot = GetNode<Node>("/root/Global");
        //GlobalVars globalVars = globalRoot.GetNode<GlobalVars>("GlobalVars");

        LevelLoader levelLoader = GetNode<LevelLoader>("/root/Global/LevelLoader");
        levelLoader.LoadGameplayLevel();
        //GetTree().ChangeScene("res://Scenes/Main.tscn");
    }

    private void OnLevelSelect()
    {
        GD.Print("OnLevelSelect");

        GetTree().ChangeScene("res://Scenes/LevelSelect.tscn");
    }

    private void OnQuitButton()
    {
        GD.Print("OnQuitButton");
        //GetTree().ChangeScene("res://Scenes/Main.tscn");
    }
}
