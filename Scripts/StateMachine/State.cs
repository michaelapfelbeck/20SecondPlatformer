public abstract class State
{
    abstract public string Description { get; }
    abstract public void Tick(float delta);
    abstract public void OnEnter();
    abstract public void OnExit();
}