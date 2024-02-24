using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine 
{
    public State currentState { get; private set; }

    public void Initialize(State state)
    {
        currentState = state;
        currentState.Enter();
    }


    public void ChangeState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();   
    }
}
