using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 5f;
    public float chargeTime = 2f;
    public float rotationToPlayerSpeed = 150f;
    public float boarderAngle = 5f;
}
