using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;


    public MeleeAttackState(FiniteStateMachine stateMachine, Entity entity, string animBoolName, Transform attackPosistion, D_MeleeAttackState satateData) : base(stateMachine, entity, animBoolName, attackPosistion)
    {
        this.stateData = satateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        SoundPool.SoundInstance.PlayVFXSound(stateData.attackSound, entity.transform.position);
        //core.Movement.SetVelocityZ(stateData.attackMovementSpeed);
    }

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
        //if (setVelocity)
        //{
        //    core.Movement.MoveX(stateData.attackMovementSpeed);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        var detectedObjects = Physics.OverlapSphere(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);
        //Debug.Log("detectedObjects " + detectedObjects.Length);
        foreach (var obj in detectedObjects)
        {
            var damageable = obj.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(stateData.attackDamage, Vector3.zero);
            }
        }
    }
}
