using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;

public class AttackChargeState : State
{

    protected Transform attackPosition;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    protected bool isDamageDone;


    protected D_AttackChargeStateData stateData;

    public AttackChargeState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosistion, D_AttackChargeStateData stateData) : base(stateMachine, entity, animBoolName)
    {
        this.attackPosition = attackPosistion;
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isDamageDone = false;
        entity.atsm.attackChargeState = this;
        isAnimationFinished = false;
        core.Movement.SetVelocity(stateData.jumpStrength, stateData.jumpAngle, core.Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        CheckForAttack();
    }

    public virtual void TriggerAttack()
    {

    }


    public virtual void FinishChargeAttack()
    {
        isAnimationFinished = true;
    }


    public virtual void CheckForAttack()
    {
        if (!isDamageDone)
        {
            var detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);
            foreach (var obj in detectedObjects)
            {
                var damageable = obj.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    //if (damageable.TakeDamage(stateData.attackDamage, entity.transform.position))
                    {
                        //damageable.Knockback(stateData.knockbackAngle, stateData.knockbackStrength, core.Movement.FacingDirection);
                        //isDamageDone = true;
                    }
                }
            }
        }
    }
}


