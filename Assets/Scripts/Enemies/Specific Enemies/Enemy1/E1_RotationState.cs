using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_RotationState : RotationState
{
    private Enemy_1 enemy;
    public E1_RotationState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_RotationState data, Enemy_1 eneny) : base(stateMachine, entity, animBoolName, data)
    {
        this.enemy = eneny;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isRotationFinished)
        {
            stateMachine.ChangeState(enemy.moveState);
        }


    }
}
