using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{

    protected bool isPlayerInMinAgroRange;
    protected bool isAnimationFinished;


    public DamageState(FiniteStateMachine stateMachine, Entity entity, string animBoolName) : base(stateMachine, entity, animBoolName)
    {

    }


    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }



    public override void Enter()
    {
        base.Enter();
        entity.atsm.damageState = this;
        isAnimationFinished = false;
    }



    public virtual void FinishDamageAnimation()
    {
        isAnimationFinished = true;
    }

}
