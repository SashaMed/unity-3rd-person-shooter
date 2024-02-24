using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRotationStateData", menuName = "Data/State Data/Rotation State")]
public class D_RotationState : ScriptableObject
{
    public float rotationSpeed = 3f;
    public float minRotationTime = 1f;
    public float maxRotationTime = 2f;
}
