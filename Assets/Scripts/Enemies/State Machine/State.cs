using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    public float startTime { get; protected set; }
    protected string animBoolName;
    protected Core core;

    public State(FiniteStateMachine stateMachine, Entity entity, string animBoolName)
    {
        core = entity.Core;
        this.stateMachine = stateMachine;
        this.entity = entity;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        //Debug.Log($"{core.transform.parent.name} {animBoolName}");
        startTime = Time.time;
        DoChecks();
        entity.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        entity.animator.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
