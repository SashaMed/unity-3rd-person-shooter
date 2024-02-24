using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;
    public DamageState damageState;
    public DodgeState dodgeState;
    public AttackChargeState attackChargeState;


    [SerializeField] private GameObject projectileAnimation;


    private void TriggerAttack()
    {
        if (attackState != null)
        {
            attackState.TriggerAttack();
        }
    }


    private void AnimationMovementStartTrigger()
    {
        if (attackState != null)
        {
            attackState.AnimationMovementStartTrigger();
        }
    }

    private void AnimationMovementStopTrigger()
    {
        if (attackState != null)
        {
            attackState.AnimationMovementStopTrigger();
        }
    }

    private void FinishAttack()
    {
        if (attackState != null)
        {
            attackState.FinishAttack();
        }
    }

    private void FinishDamageAnimation()
    {
        if (damageState != null)
        {
            damageState.FinishDamageAnimation();
        }
    }

    private void FinishChargeAttack()
    {
        if (attackChargeState != null)
        {
            attackChargeState.FinishChargeAttack();
        }
    }


    private void TriggerChargeAttack()
    {
        if (attackChargeState != null)
        {
            attackChargeState.TriggerAttack();
        }
    }


    private void FinishDodgeAnimation()
    {
        if (dodgeState != null)
        {
            dodgeState.FinishDodgeAnimation();
        }
    }


    public void SetActiveProjectile(bool fl)
    {
        if (projectileAnimation != null)
        {
            projectileAnimation.SetActive(fl);
        }
    }
}
