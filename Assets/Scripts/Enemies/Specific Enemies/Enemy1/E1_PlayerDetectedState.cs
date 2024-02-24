using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy_1 enemy;

    public E1_PlayerDetectedState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_PlayerDetectedState stateData, Enemy_1 enemy) : 
        base(stateMachine, entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Enter PlayerDetectedState state");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (performCloseRangeAction)
        {
            //Debug.Log("player deteccted stateMachine.ChangeState(enemy.meleeAttackState)");
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (performLongRangeAction)
        {
            //Debug.Log("player deteccted stateMachine.ChangeState(enemy.chargeState);");
            enemy.chargeState.SetPlayerPosition(enemy.GetPlayerPosition());
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            //Debug.Log("player deteccted stateMachine.ChangeState(enemy.lookForPlayerState);");
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        //else if (!isDetectiongLedge && !isPlayerInMaxAgroRange)
        //{
        //    //core.Movement.Flip();
        //    //Debug.Log("player deteccted stateMachine.ChangeState(enemy.moveState);");
        //    stateMachine.ChangeState(enemy.moveState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
