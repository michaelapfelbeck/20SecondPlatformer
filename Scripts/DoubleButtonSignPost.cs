using Godot;

[Tool]
public class DoubleButtonSignPost : Node2D
{
    [Export]
    public Texture KeyboardTextureLeft;
    [Export]
    public Texture KeyboardTextureRight;
    [Export]
    public Texture ControllerTextureLeft;
    [Export]
    public Texture ControllerTextureRight;

    [Export]
    public string Text;

    [Export]
    public string inputActionLeft;

    [Export]
    public string inputActionRight;

    [Export]
    public Color inputTint;

    [Export]
    public bool SimulateControllerConnected;

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
        Sprite leftSprite = GetNode<Sprite>("LeftSprite");
        Sprite rightSprite = GetNode<Sprite>("RightSprite");
        if (leftSprite == null || rightSprite == null)
        {
            return;
        }

        if (Input.GetConnectedJoypads().Count != 0 || SimulateControllerConnected)
        {
            leftSprite.Texture = ControllerTextureLeft;
            rightSprite.Texture = ControllerTextureRight;
        }
        else
        {
            leftSprite.Texture = KeyboardTextureLeft;
            rightSprite.Texture = KeyboardTextureRight;
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
            if (inputActionLeft != null && inputTint != null && Input.IsActionPressed(inputActionLeft))
            {
                GetNode<Sprite>("LeftSprite").Modulate = inputTint;
            }
            else
            {
                GetNode<Sprite>("LeftSprite").Modulate = Colors.White;
            }

            if (inputActionRight != null && inputTint != null && Input.IsActionPressed(inputActionRight))
            {
                GetNode<Sprite>("RightSprite").Modulate = inputTint;
            }
            else
            {
                GetNode<Sprite>("RightSprite").Modulate = Colors.White;
            }
        }
    }
}
