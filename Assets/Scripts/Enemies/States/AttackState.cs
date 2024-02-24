using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    
    protected Transform attackPosition;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    protected bool setVelocity;
    protected float lastAttackTime;

    public AttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosistion) : base(stateMachine, entity, animBoolName)
    {
        this.attackPosition = attackPosistion;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        entity.atsm.attackState = this;
        isAnimationFinished = false;
        setVelocity = false;
        core.Movement.MoveX(0); 
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core.Movement.MoveX(0);
    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        isAnimationFinished=true;
    }


    public virtual void AnimationMovementStartTrigger()
    {
        setVelocity = true;
    }

    public virtual void AnimationMovementStopTrigger()
    {
        setVelocity = false;
    }
}
