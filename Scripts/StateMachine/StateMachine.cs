using Godot;
using System;
using System.Collections.Generic;

public class StateMachine<T> where T : State
{
    public string StateDescription { get => currentState.Description; }
    public T CurrentState { get => currentState; }
    public bool Debug { get; set; }
    protected T currentState;

    private Dictionary<T, List<Transition>> transitionsTable = new Dictionary<T, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    // TODO: Add state transition priority
    public void At(T from, T to, Func<bool> condition, string transitionLabel = null) => AddTransition(from, to, condition, transitionLabel);

    public void Tick(float delta)
    {
        var transition = GetTransition();
        if (transition != null)
        {
            if (Debug)
            {
                if (transition.Label != null)
                {
                    GD.Print(String.Format("State transition {0} -> {1} triggered by {2}", currentState != null ? currentState.Description : "(null)", transition.To.Description, transition.Label));
                }
                else
                {
                    GD.Print(String.Format("State transition {0} -> {1}", currentState != null ? currentState.Description : "(null)", transition.To.Description));
                }
            }
            SetState(transition.To);
        }

        currentState?.Tick(delta);
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

    public void AddTransition(T from, T to, Func<bool> predicate, string transitionLabel = null)
    {
        if (transitionsTable.TryGetValue(from, out var transitions) == false)
        {
            transitions = new List<Transition>();
            transitionsTable
                [from] = transitions;
        }

        transitions.Add(new Transition(to, predicate, transitionLabel));
    }

    public void AddAnyTransition(T state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public T To { get; }
        public string Label { get; }

        public Transition(T to, Func<bool> condition, string label = null)
        {
            To = to;
            Condition = condition;
            Label = label;
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