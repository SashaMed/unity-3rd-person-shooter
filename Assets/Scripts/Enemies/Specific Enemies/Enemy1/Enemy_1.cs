using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Entity
{


    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_MeleeAttackState meleeAttackState { get; private set; }
    public E1_StunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }
    public E1_DamageState damageState { get; private set; }
    public E1_RotationState rotationState { get; private set; }

    public bool isInExtremeRange;

    [SerializeField] private D_RotationState rotationStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private Transform meleeAttackPosition;

    private EnemyHealth healthSystem;

    public override void Awake()
    {
        base.Awake();
        chargeState = new E1_ChargeState(stateMachine, this, "charge", chargeStateData, this);
        moveState = new E1_MoveState(stateMachine, this, "move", moveStateData, this);
        idleState = new E1_IdleState(stateMachine, this, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(stateMachine, this, "idle", playerDetectedStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(stateMachine, this, "move", lookForPlayerStateData, this);
        meleeAttackState = new E1_MeleeAttackState(stateMachine, this, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E1_StunState(stateMachine, this, "stun", stunStateData, this);
        deadState = new E1_DeadState(stateMachine, this, "dead", deadStateData, this);
        damageState = new E1_DamageState(stateMachine, this, "damage", this);
        rotationState = new E1_RotationState(stateMachine, this, "move", rotationStateData, this);
        healthSystem = GetComponent<EnemyHealth>(); 


    }

    protected override void Start()
    {
        base.Start();
        healthSystem.OnDeath += DeathHandler;
        healthSystem.OnTakeDamage += DamageHandler;
        stateMachine.Initialize(moveState);
    }



    private void DeathHandler(Vector3 pos)
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        Instantiate(entityData.destroyParticle, meleeAttackPosition.position, Quaternion.identity);
        SoundPool.SoundInstance.PlayVFXSound(entityData.destroySound, pos);
        stateMachine.ChangeState(deadState);
    }



    private void DamageHandler(float damage, Vector3 pos)
    {
        SoundPool.SoundInstance.PlayVFXSound(entityData.damageSound, pos);
        ParticlePool.Instance.BloodParticlesPool.GetFromPool(pos);
        if (isDead)
        {
            return;
        }
        if (stateMachine.currentState == idleState || stateMachine.currentState == moveState)
        {

            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void Update()
    {
        base.Update();
        isInExtremeRange = CheckPlayerCloseBehind();
    }

}
