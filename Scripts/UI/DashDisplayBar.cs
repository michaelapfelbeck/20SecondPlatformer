using Godot;

public class DashDisplayBar : ProgressBar
{
    private PlayerController controller;

    public override void _Ready()
    {
        controller = GetTree().Root.GetNode<PlayerController>("Main/Player");
    }

    //private float acc = 0f;

    public override void _Process(float delta)
    {
        if(controller == null)
        {
            return;
        }
        float dashRemaining = Mathf.Max(0, controller.dashDuration - controller.dashTime);
        float progressBarValue = ReMap(dashRemaining, 0, controller.dashDuration, (float)MinValue, (float)this.MaxValue);
        //if (acc > 1f)
        //{
        //    GD.Print("*****");
        //    GD.Print(String.Format("Dash time: {0}", controller.dashTime));
        //    GD.Print(String.Format("Dash duration: {0}", controller.dashDuration));
        //    GD.Print(String.Format("min value time: {0}", MinValue));
        //    GD.Print(String.Format("max value: {0}", MaxValue));
        //    GD.Print(String.Format("Dash remaining: {0}", dashRemaining));
        //    GD.Print(String.Format("progress Bar Value: {0}", progressBarValue));
        //    acc = 0;
        //}
        //else
        //{
        //    acc += delta;
        //}
        this.Value = progressBarValue;
    }

    public float ReMap(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * (value - istart) / (istop - istart);
    }
}
