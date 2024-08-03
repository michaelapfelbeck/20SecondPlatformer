using Godot;
using System;

public class FramerateDisplay : Label
{
	[Export]
	public float updatesPerSecond = 1;

	private float updateRate;
	private float timerAcc;
	private float fpsAcc;
	private float updateCount;
	private string template = "FPS: {0}";
	public override void _Ready()
	{
		DeriveVariables();
		timerAcc = 0;
		fpsAcc = 0;
		Text = "";
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		fpsAcc += Engine.GetFramesPerSecond();
		timerAcc += delta;
		updateCount += 1;
		if(timerAcc >= updateRate)
		{
			UpdateCounter();
			fpsAcc = 0;
			updateCount = 0;
			timerAcc -= updateRate;
		}

		if (Input.IsActionJustPressed("fullscreen"))
		{
			ToggleFullscreen();
		}
	}

	private void ToggleFullscreen()
	{
		GD.Print("Toggle Fullscreen");
		OS.WindowFullscreen = !OS.WindowFullscreen;
	}

	private void DeriveVariables()
	{
		updateRate = 1.0f / updatesPerSecond;
	}

	private void UpdateCounter()
	{
		if(updateCount == 0)
		{
			return;
		}
		int result = (int)(fpsAcc / updateCount);
		try
		{
			Text = String.Format(template, result);
		}
		catch
		{
			Text = result.ToString();
		}
		//GD.Print(String.Format(template, (int)result));
		//GD.Print("actual: " + Engine.GetFramesPerSecond());
	}
}
