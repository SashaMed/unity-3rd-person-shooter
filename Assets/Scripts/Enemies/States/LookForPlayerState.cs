using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State

{
    protected D_LookForPlayerState stateData;
    protected bool turnImmediately;
    protected bool isPlayerInAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;
    protected bool isPlayerCloseBehind;

    protected float lastTurnTime;
    protected int amountOfTurnsDone;
    protected int rotationDirection;


    public LookForPlayerState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_LookForPlayerState stateData) :
        base(stateMachine, entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInAgroRange = entity.CheckPlayerInFarRange();
        isPlayerCloseBehind = entity.CheckPlayerCloseBehind();
    }

    public override void Enter()
    {
        base.Enter();
        entity.AiAgent.StopMove();
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        lastTurnTime = startTime;
        amountOfTurnsDone = 0;
        rotationDirection = Random.Range(0, 9) % 2 == 0 ? 1 : -1;
        //core.Movement.MoveX(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //core.Movement.SetVelocityX(0);
        if (turnImmediately)
        {
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            //core.Movement.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            rotationDirection *= -1;
        }

        if (amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }


        //if (Time.time > startTime + rotationTime)
        //{
        //    isRotationFinished = true;
        //}

        core.Movement.RotateY(stateData.rotationSpeed * rotationDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip;
    }
}
