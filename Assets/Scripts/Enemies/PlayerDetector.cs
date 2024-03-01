using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour, IPlayerDetector
{
    [SerializeField] private Transform head;
    [SerializeField] private float minAgroDetectionRadius = 7f;
    [SerializeField] private float maxAgroDetectionRadius = 13;
    [SerializeField] private float closeBehindDetectionRadius = 3;
    [SerializeField] private float closeRangeDetectionRadius = 3;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private LayerMask obstacleMask;

    public bool PlayerDetectedInCloseRange { get; private set; }
    public bool PlayerDetected { get; private set; }
    public bool IsPlayerInMinAgroRange { get; private set; }
    public bool IsPlayerInMaxAgroRange { get; private set; }
    public bool IsPlayerInCloseRange { get; private set; }
    public bool isPlayerCloseBehind { get; private set; }

    //public bool PlayerDetectedInCloseRange { get; private set; }

    public Vector3 PlayerPosition { get; private set; }
    public Transform PlayerTransform { get; private set; }

    private Transform player;


    public virtual bool CheckPlayerInMinAgroRange()
    {
        return DetectedPlayer(minAgroDetectionRadius);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return DetectedPlayer(maxAgroDetectionRadius);
    }

    public virtual bool CheckPlayerInCloseRange()
    {
        return DetectedPlayer(closeRangeDetectionRadius);
    }


    public virtual bool CheckPlayerInFarRange()
    {
        return DetectedPlayer(maxAgroDetectionRadius * 2);
    }

    public virtual bool CheckPlayerCloseBehind()
    {
        if (player == null)
        {
            return false;
        }
        var distanceToPlayer = Vector3.Distance(head.position, player.position);
        if (distanceToPlayer >= closeBehindDetectionRadius)
        {
            return false;
        }
        return true;
    }

    private bool DetectedPlayer(float detectedDistance)
    {
        if (player == null)
        {
            return false;
        }
        var directionToPlayer = player.position - head.position;
        var distanceToPlayer = Vector3.Distance(head.position, player.position);
        if (distanceToPlayer >= detectedDistance)
        {
            return false;
        }
        var angleToPlayer = Vector3.Angle(head.forward, directionToPlayer);
        if (angleToPlayer >= detectionAngle)
        {
            return false;
        }
        if (!Physics.Raycast(head.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask))
        {
            return true;

        }
        return false;
    }

    public Vector3 GetPlayerPosition() => player.position;

    public Transform GetPlayerTransform() => player;

    //private void Update()
    //{
    //    DetectPlayer();
    //}


    private void OnDrawGizmosSelected()
    {
        if (head == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(head.position, closeRangeDetectionRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(head.position, closeBehindDetectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(head.position, minAgroDetectionRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(head.position, maxAgroDetectionRadius * 2);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(head.position, maxAgroDetectionRadius);
        Vector3 fovLine1 = Quaternion.AngleAxis(detectionAngle, head.up) * head.forward * maxAgroDetectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-detectionAngle, head.up) * head.forward * maxAgroDetectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(head.position, fovLine1);
        Gizmos.DrawRay(head.position, fovLine2);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
}
