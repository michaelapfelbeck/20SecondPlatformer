using Godot;

public class LevelSelect : Control
{
    [Export]
    public NodePath defaultFocus;
    private bool firstFocus = false;
    public override void _Ready()
    {
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if(!firstFocus)
        {
            SetDefaultFocus();
            firstFocus = true;
        }
    }

    private void SetDefaultFocus()
    {
        GD.Print("LevelSelect: SetDefaultFocus");
        if (defaultFocus != null)
        {
            Control focusNode = GetNode<Control>(defaultFocus);
            focusNode.GrabFocus();
        }
    }
}
