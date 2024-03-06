using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetection : MonoBehaviour, IPlayerDetector
{
    [SerializeField] private Transform turretHead;
    [SerializeField] private float closeDetectionRadius = 7f;
    [SerializeField] private float extremeCloseDetectionRadius = 3;
    [SerializeField] private float detectionRadius = 13;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    public bool PlayerDetectedInBehind { get; private set; }
    public bool PlayerDetectedInCloseRange { get; private set; }
    public bool PlayerDetected { get; private set; }
    public Vector3 PlayerPosition { get; private set; }


    private Transform player;


    private void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        PlayerDetected = false;
        PlayerDetectedInBehind = false;
        PlayerDetectedInCloseRange = false;
        PlayerPosition = new Vector3();
        if (player == null)
        {
            return;
        }
        var directionToPlayer = player.position - turretHead.position;
        var distanceToPlayer = Vector3.Distance(turretHead.position, player.position);
        if (distanceToPlayer >= detectionRadius)
        {
            PlayerDetected = false;
            PlayerPosition = new Vector3();
            return;
        }
        var angleToPlayer = Vector3.Angle(turretHead.forward, directionToPlayer);
        if (angleToPlayer <= detectionAngle)
        {

            if (!Physics.Raycast(turretHead.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask))
            {
                PlayerPosition = player.position;
                PlayerDetected = true;
                if (distanceToPlayer < closeDetectionRadius)
                {
                    PlayerDetectedInCloseRange = true;
                }
                return;

            }
        }

        if (distanceToPlayer <= extremeCloseDetectionRadius)
        {
            PlayerPosition = player.position;
            PlayerDetectedInBehind = true;
            PlayerDetected = true;
            return;
        }
        PlayerDetected = false;
        PlayerDetectedInBehind = false;
        PlayerDetectedInCloseRange = false;
        PlayerPosition = new Vector3();
    }


    private void OnDrawGizmosSelected()
    {
        if (turretHead == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(turretHead.position, closeDetectionRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(turretHead.position, extremeCloseDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(turretHead.position, detectionRadius);
        Vector3 fovLine1 = Quaternion.AngleAxis(detectionAngle, turretHead.up) * turretHead.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-detectionAngle, turretHead.up) * turretHead.forward * detectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(turretHead.position, fovLine1);
        Gizmos.DrawRay(turretHead.position, fovLine2);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
}
