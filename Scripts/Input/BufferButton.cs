using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BufferButton
{
    private string button;
    private float timeout;
    private float pressTime;
    private bool pressed;

    public bool JustPressed { get => pressed && pressTime <= timeout; }
    public bool Pressed { get => pressed; }
    public bool Released { get => !pressed; }

    public bool ConsumedThisFrame { get; private set; }

    public void Reset()
    {
        pressed = false;
        pressTime = 0;
    }
    public BufferButton(string label, float bufferLifespan)
    {
        button = label;
        timeout = bufferLifespan;
        Reset();
    }

    public void TIck(float delta)
    {
        ConsumedThisFrame = false;
        if (Input.IsActionJustPressed(button)){
            pressTime = 0;
            pressed = true;
        } else if (Input.IsActionPressed(button))
        {
            pressTime += delta;
        }
        else
        {
            Reset();
        }
    }

    public void Consume()
    {
        pressTime = float.MaxValue;
        ConsumedThisFrame = true;
    }
}