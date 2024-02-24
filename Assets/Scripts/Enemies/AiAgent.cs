using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    private Transform target;
    private bool move;
    NavMeshAgent agent;
    public bool isReachedTarget = false;
    public float stopDistance = 0.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform transform, float speed = 0, float rotationSpeed = 0)
    {
        isReachedTarget = false;
        move = true;
        target = transform;
        agent.isStopped = false;
        if (speed > 0 && rotationSpeed > 0)
        {
            agent.speed = speed;
            agent.angularSpeed = rotationSpeed;
        }
    }

    public bool IsReachedTarget()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= stopDistance;
    }

    public void StopMove()
    {
        agent.isStopped = true;
        move = false;
        target = null;
    }

    private void Update()
    {
        if (target != null && move)
        {
            agent.SetDestination(target.position); 

            
            if (/*!isReachedTarget && */Vector3.Distance(transform.position, target.position) <= stopDistance)
            {
                agent.isStopped = true; 
                isReachedTarget = true; 
            }
            else if (/*isReachedTarget && */Vector3.Distance(transform.position, target.position) > stopDistance)
            {
                agent.isStopped = false; 
                isReachedTarget = false; 
            }
        }
    }
}
