using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DamageState : DamageState
{

    private Enemy_1 enemy;
    public E1_DamageState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Enemy_1 enemy) :
    base(stateMachine, entity, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

    }


    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

}
