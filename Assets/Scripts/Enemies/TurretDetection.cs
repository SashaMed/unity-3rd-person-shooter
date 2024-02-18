using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    [SerializeField] private Transform turretHead;
    [SerializeField] private float closeDetectionRadius = 3f;
    [SerializeField] private float detectionRadius = 13;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;


    public bool PlayerDetected { get; private set; }
    public Vector3 PlayerPosition { get; private set; }


    private Transform player;

    private void Start()
    {
        var playerScript = (PlayerController)FindObjectOfType(typeof(PlayerController));
        player = playerScript.gameObject.transform;
    }

    private void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
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
                return;

            }
        }

        if (distanceToPlayer <= closeDetectionRadius)
        {
            PlayerPosition = player.position;
            PlayerDetected = true;
            return;
        }
        PlayerDetected = false;
        PlayerPosition = new Vector3();
    }


    private void OnDrawGizmosSelected()
    {
        if (turretHead == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(turretHead.position, closeDetectionRadius);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(turretHead.position, detectionRadius);
        Vector3 fovLine1 = Quaternion.AngleAxis(detectionAngle, turretHead.up) * turretHead.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-detectionAngle, turretHead.up) * turretHead.forward * detectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(turretHead.position, fovLine1);
        Gizmos.DrawRay(turretHead.position, fovLine2);
    }
}