using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy_1 enemy;
    public E1_ChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, D_ChargeState stateData, Enemy_1 enemy) : 
        base(stateMachine, entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Enter charge state");
        //RotateToPlayer();
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
            stateMachine.ChangeState(enemy.meleeAttackState);
        }

        //else if (/*!isDetectiongLedge ||*/ isDetectiongWall) 
        //{
        //    //Debug.Log("E1_ChargeState stateMachine.ChangeState(enemy.lookForPlayerState);");
        //    stateMachine.ChangeState(enemy.lookForPlayerState);
        //}

        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAgroRange)
            {
                //Debug.Log("E1_ChargeState stateMachine.ChangeState(enemy.playerDetectedState);");
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                //Debug.Log(" isChargeTimeOver E1_ChargeState stateMachine.ChangeState(enemy.lookForPlayerState);");
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
            //RotateToPlayer();
    }

    private void RotateToPlayer()
    {
        var directionToPlayer = playerPosition - enemy.transform.position;
        var angleToPlayer = Vector3.Angle(enemy.transform.forward, directionToPlayer);
        var lookRotation = Quaternion.LookRotation(directionToPlayer);
        //var angle = Quaternion.Eu(lookRotation);
        Debug.Log("angleToPlayer: " + angleToPlayer + ", directionToPlayer: " + directionToPlayer);
        if (angleToPlayer < stateData.boarderAngle)
        {
            return;
        }
        var dir = (playerPosition.x > enemy.transform.position.x) ? 1 : -1;
        enemy.transform.Rotate(0, angleToPlayer * dir, 0);
        //enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, lookRotation, Time.deltaTime * stateData.rotationToPlayerSpeed);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
