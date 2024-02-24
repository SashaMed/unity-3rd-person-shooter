using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected bool isDetectiongWall;
    protected bool isDetectiongLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerCloseBehind;
    protected bool isPlayerInMaxAgroRange;
    protected bool IsReachedTarget;
    protected D_MoveState stateData;

    public MoveState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_MoveState stateData) : base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
         
        isDetectiongLedge = core.CollisionSenses.CheckIsLedgeAhead();
        isDetectiongWall = core.CollisionSenses.CheckIsTouchingWall();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerCloseBehind = entity.CheckPlayerCloseBehind();

            IsReachedTarget = entity.AiAgent.IsReachedTarget();
        
    }

    public override void Enter()
    {
        base.Enter();
        SetRandomTargetPosition(20);
        entity.AiAgent.SetTarget(entity.tempTarget, stateData.movementSpeed, stateData.rotationSpeed);
        //core.Movement.MoveZ(stateData.movementSpeed );
    }



    public override void Exit()
    {
        base.Exit();
        entity.AiAgent.StopMove();

    }

    private void SetRandomTargetPosition(float radius)
    {
        var randomDirection = Random.insideUnitSphere * radius;
        randomDirection += entity.tempTarget.position; 
        randomDirection.y = entity.tempTarget.position.y;

        entity.tempTarget.position = randomDirection;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //core.Movement.MoveZ(stateData.movementSpeed );
        //core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

