using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class D_MeleeAttackState : ScriptableObject
{
    public int attacksCount = 1;
    public float attackDamage = 10;
    public float attackRadius = 0.5f;
    public float attackMovementSpeed = 5f;
    public LayerMask whatIsPlayer;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;
    public AudioClip attackSound;
    public float attackSoundVolume = 0.5f;
}
