using System;
using System.Collections.Generic;

public class StateMachine<T> where T : State
{
    public string StateDescription { get => currentState.Description; }
    public T CurrentState { get => currentState; }
    protected T currentState;

    private Dictionary<T, List<Transition>> transitionsTable = new Dictionary<T, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public void At(T from, T to, Func<bool> condition) => AddTransition(from, to, condition);

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        currentState?.Tick();
    }

    public void SetState(T state)
    {
        if (state == currentState)
            return;

        currentState?.OnExit();
        currentState = state;

        transitionsTable.TryGetValue(currentState, out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = EmptyTransitions;

        currentState.OnEnter();
    }

    public void AddTransition(T from, T to, Func<bool> predicate)
    {
        if (transitionsTable.TryGetValue(from, out var transitions) == false)
        {
            transitions = new List<Transition>();
            transitionsTable
                [from] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(T state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public T To { get; }

        public Transition(T to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}