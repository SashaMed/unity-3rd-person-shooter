using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    [Header("Stats")]
    public float damageHopSpeed = 3f;
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    [Header("Check variables")]
    public float wallCheckDistance = 0.3f;
    public float obstaclesCheckDistance = 0.6f;
    public float ledgeCheckDistance = 0.5f;
    public float closeRangeActionDistance = 1f;
    public float extremeCloseRangeActionDistance = 0.2f;
    public float maxAgroDistance = 4f;
    public float minAgroDistance = 3f;
    public float groundCheckRadius = 0.3f;

    [Header("Cooldowns")]
    public float meleeAttackCooldown = 1f;
    public float rangeAttackCooldown = 1f;


    [Header("Layers")]
    public LayerMask whatIsObstacles;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;

    [Header("Collision Ignore variables")]
    public float collisionIgnoreDuration = 1f;
    public float collisionNotIgnoreDuration = 0.5f;

    [Header("Sounds")]
    public AudioClip damageSound;
    public AudioClip destroySound;

    [Header("Particles")]
    public GameObject destroyParticle;
}
