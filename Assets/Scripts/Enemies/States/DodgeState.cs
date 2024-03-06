using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    public DodgeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_DodgeState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isGrounded = core.CollisionSenses.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        isDodgeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public virtual void FinishDodgeAnimation()
    {
        isDodgeOver = true;
    }
}
