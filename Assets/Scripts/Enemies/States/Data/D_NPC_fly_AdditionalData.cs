using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/NPC fly Data")]

public class D_NPC_fly_AdditionalData : ScriptableObject
{
    public float velocityY = 1f;
    public float setVelocityInterval = 0.5f;


    public float playerCheckCircleRadius = 5f;
    public float initialCircleRadius = 4f;

    public float flipCooldown = 0.3f;
    public float flipRandomAngle = 180;

    [Header("Player Detected State")]
    public float minDistanceToPlayer = 1f;
    public float playerDetectedVelocityX = 0.5f;
    public float overtakeGravityMultiplier = 2f;
    public float notOvertakeGravityMultiplier = 1.5f;
    public float defaultGravityMultiplier = 1f;
}

