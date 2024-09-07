using Godot;
using System;

//[Tool]
public class LevelSelectElement : Control
{
    [Export]
    public NodePath iconPath;

    [Export]
    public NodePath textPath;

    private Sprite icon;

    private Label text;

    [Export]
    public LevelContainer data;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //GD.Print("_Ready");
        Setup(data);
    }

    //public override void _Process(float delta)
    //{
    //    base._Process(delta);
    //    if (Engine.EditorHint)
    //    {
    //        Setup(data);
    //    }
    //}

    public void Setup(LevelContainer newData)
    {
        try
        {
            icon = GetNode<Sprite>(iconPath);
            text = GetNode<Label>(textPath);

            if (data != null)
            {
                AssignData(newData);
            }
        } catch (Exception ex)
        {
            GD.Print($"LevelSelectElement Setup failed, reason: {ex.Message}");
        }
    }

    private void AssignData(LevelContainer newData)
    {
        if (text != null) 
        { 
            text.Text = newData.Name;
        }
        if (icon != null)
        {
            icon.Texture = newData.Icon;
        }
    }
}
