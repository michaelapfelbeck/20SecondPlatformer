using Godot;
using System;

[Tool]
public class SignPost : Node2D
{
    [Export]
    private Texture KeyboardTexture;
    [Export]
    private Texture ControllerTexture;

    [Export]
    private string Text;

    [Export]
    private bool SimulateControllerConnected;

    [Export]
    private string inputAction;

    [Export]
    private Color inputTint;

    public override void _Ready()
    {
        SetSprite();
        SetText();
    }

    private void SetText()
    {
        Label label = GetNode<Label>("Label");
        if (label == null)
        {
            return;
        }

        label.Text = Text;
    }

    private void SetSprite()
    {
        Sprite inputSprite = GetNode<Sprite>("Sprite");
        if (inputSprite == null)
        {
            return;
        }

        if (Input.GetConnectedJoypads().Count != 0 || SimulateControllerConnected)
        {
            inputSprite.Texture = ControllerTexture;
        }
        else
        {
            inputSprite.Texture = KeyboardTexture;
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Engine.EditorHint)
        {
            SetSprite();
            SetText();
        }

        if (!Engine.EditorHint)
        {
            if (inputAction != null && inputTint != null && Input.IsActionPressed(inputAction))
            {
                GetNode<Sprite>("Sprite").Modulate = inputTint;
            }
            else
            {
                GetNode<Sprite>("Sprite").Modulate = Colors.White;
            }
        }
    }

    //public override bool _Set(string property, object value)
    //{
    //    GD.Print($"set {property}");
    //    return base._Set(property, value);
    //}
}
