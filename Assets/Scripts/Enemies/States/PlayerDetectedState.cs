using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerCloseBehind;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectiongLedge;


    public PlayerDetectedState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_PlayerDetectedState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectiongLedge = core.CollisionSenses.CheckIsLedgeAhead(); //| entity.CheckObstacles();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInFarRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRange();
        isPlayerCloseBehind = entity.CheckPlayerCloseBehind();
    }

    public override void Enter()
    {
        base.Enter();
        performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.longRagneAction)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
