using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLooForPlayerStateData", menuName = "Data/State Data/Look For PLayer State")]
public class D_LookForPlayerState : ScriptableObject
{
    public float rotationSpeed = 5f;
    public int amountOfTurns = 2;
    public float timeBetweenTurns = 0.75f;
}
