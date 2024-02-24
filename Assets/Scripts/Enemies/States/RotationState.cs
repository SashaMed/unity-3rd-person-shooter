using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationState : State
{
    protected float rotationTime;
    protected int rotationDirection;
    protected bool isRotationFinished;
    protected D_RotationState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerCloseBehind;

    public RotationState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_RotationState data) : base(stateMachine, entity, animBoolName)
    {
        stateData = data;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerCloseBehind = entity.CheckPlayerCloseBehind();
    }

    public override void Enter()
    {
        base.Enter();
        isRotationFinished = false;
        rotationTime = Random.Range(stateData.minRotationTime, stateData.maxRotationTime);
        rotationDirection = Random.Range(0, 9) % 2 ==  0 ? 1 : -1;
        if (rotationDirection == 1)
        {
            Debug.Log("right"); 
        }
        else
        {
            Debug.Log("left");
        }
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time > startTime + rotationTime)
        {
            isRotationFinished = true;
        }

        core.Movement.RotateY(stateData.rotationSpeed * rotationDirection);
    }


}
