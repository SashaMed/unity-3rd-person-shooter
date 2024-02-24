using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newAttackChargeStateData", menuName = "Data/State Data/Attack Charge State")]
public class D_AttackChargeStateData : ScriptableObject
{


    public float returnAfterAttackDistance = 0.4f;
    public float attackChargeTime = 2f;

    public Vector2 jumpAngle = Vector2.one;
    public float jumpStrength = 10f;

    public AudioClip attackSound;
    public float attackSoundVolume = 0.5f;

    public float attackDamage = 10;
    public float knockbackStrength = 8;
    public float attackRadius = 0.5f;
    public LayerMask whatIsPlayer;
    public Vector2 knockbackAngle = Vector2.one;
    
}
