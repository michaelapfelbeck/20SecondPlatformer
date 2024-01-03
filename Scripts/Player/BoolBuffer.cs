using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoolBuffer
{
    private Func<bool> predicate;
    private float timeout;

    private float setTime;
    private bool set;

    public bool Value { get => set; }
    public BoolBuffer(Func<bool> predicate, float timeout)
    {
        this.predicate = predicate;
        this.timeout = timeout;
        Clear();
    }

    public void Clear()
    {
        setTime = 0;
        set = false;
    }

    public void TIck(float delta)
{
        if (predicate())
        {
            set = true;
            setTime = 0;
            return;
        }
        setTime += delta;
        if(set && setTime >= timeout)
        {
            Clear();
        }
    }
}
