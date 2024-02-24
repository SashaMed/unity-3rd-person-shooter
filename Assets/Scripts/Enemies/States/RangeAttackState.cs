
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState stateData;
    protected GameObject projectile;
    //protected IProjectile projectileScript;

    public RangeAttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosistion, D_RangeAttackState stateData) : 
        base(stateMachine, entity, animBoolName, attackPosistion)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        if (entity.CheckPlayerInMaxAgroRange())
        {
            //core.SoundComponent.PlayAudio(stateData.rangeAttackClip);
        }
    }

    public bool CanAttack() => (Time.time >= lastAttackTime + entity.entityData.rangeAttackCooldown);

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        //projectileScript = projectile.GetComponent<IProjectile>();
        //projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage, 0, knockbackStrenght:stateData.knockbackStrength,knockbackAngle:stateData.knockbackAngle);

    }
}
