using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ChargeState : State
{
    protected D_ChargeState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectiongWall;
    protected bool isDetectiongLedge;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;
    protected Vector3 playerPosition;

    public ChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_ChargeState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectiongLedge = core.CollisionSenses.CheckIsLedgeAhead();// | entity.CheckObstacles();
        isDetectiongWall = core.CollisionSenses.CheckIsTouchingWall(); // | entity.CheckObstacles();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRange();
    }

    public override void Enter()
    {
        base.Enter();
        isChargeTimeOver = false;
        entity.AiAgent.SetTarget(entity.GetPlayerTransform(), stateData.chargeTime, stateData.rotationToPlayerSpeed);
        //core.Movement.MoveZ(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        entity.AiAgent.StopMove();
        playerPosition = Vector3.zero;
    }

    public void SetPlayerPosition(Vector3 player)
    {
        playerPosition = player;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core.Movement.MoveZ(stateData.chargeSpeed);
        if (Time.time >= startTime + stateData.chargeTime)
        {
            entity.AiAgent.StopMove();
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}
