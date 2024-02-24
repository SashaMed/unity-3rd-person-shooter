using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform tempTarget;
    [SerializeField] protected int startFacingDirection = 0;
    [SerializeField] protected Transform playerCheck;
    [SerializeField] private Transform obstaclesCheck;

    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public Animator animator { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Core Core { get; private set; }

    public AiAgent AiAgent { get; private set; }


    private float lastPlayerCollisionTime;
    private float lastDamageTime;
    private Vector2 velocityWorkSpace;

    protected bool isDead;
    protected bool isStunned;

    public bool IsStunned => isStunned;

    private PlayerDetector detection;


    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        animator = GetComponentInChildren<Animator>();
        stateMachine = new FiniteStateMachine();
        atsm = GetComponentInChildren<AnimationToStateMachine>();
        detection = GetComponent<PlayerDetector>();
        AiAgent = GetComponent<AiAgent>();
    }

    protected virtual void Start()
    {
        //Core.Stats.onHealthZero += SetIsDead;
    }

    protected virtual void SetIsDead()
    {
        isDead = true;
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        //animator.SetFloat("yVelocity", Core.Movement.Rigidbody.velocity.y);
    }


    private void OnTriggerEnter(Collider collision)
    {
        var col = collision.GetComponent<ICollectible>();
        if (col != null)
        {
            Physics.IgnoreCollision(collision, GetComponent<Collider>());
            //col.Collect();
        }
    }

    protected IEnumerator PlayerCollisionIgnoringCoroutine(GameObject player, float ignoreDuration, float notIgnoreDuration)
    {
        var plCol = player.GetComponent<Collider2D>();
        var col = GetComponent<Collider2D>();
        yield return new WaitForSeconds(notIgnoreDuration);
        Physics2D.IgnoreCollision(plCol, col, true);
        yield return new WaitForSeconds(ignoreDuration);
        Physics2D.IgnoreCollision(plCol, col, false);
    }

    private void OnEnable()
    {
        if (isDead)
        {
            gameObject.SetActive(false);
            return;
        }
        if (stateMachine.currentState != null)
        {
            stateMachine.ChangeState(stateMachine.currentState);
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return detection.CheckPlayerInMinAgroRange(); // Physics.Raycast(playerCheck.position, transform.forward, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return detection.CheckPlayerInMaxAgroRange();//Physics.Raycast(playerCheck.position, transform.forward, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInFarRange()
    {
        return detection.CheckPlayerInFarRange();//Physics.Raycast(playerCheck.position, transform.forward, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }


    public virtual bool CheckPlayerInCloseRange()
    {
        return detection.CheckPlayerInCloseRange();// Physics.Raycast(playerCheck.position, transform.forward, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }


    public virtual bool CheckPlayerCloseBehind()
    {
        return  detection.CheckPlayerCloseBehind();//(playerCheck.position, -transform.forward, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual Vector3 GetPlayerPosition()
    {
        return detection.GetPlayerPosition();
    }

    public virtual Transform GetPlayerTransform()
    {
        return detection.GetPlayerTransform();
    }

    public virtual bool CheckObstacles()
    {
        var obs = Physics.OverlapSphere(obstaclesCheck.position, entityData.obstaclesCheckDistance, entityData.whatIsObstacles);
        return (obs != null);
        //return Physics2D.Raycast(obstaclesCheck.position, transform.right, entityData.obstaclesCheckDistance, entityData.whatIsObstacles);
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
    }


    public virtual void SetIsStunned()
    {
        isStunned = true;
    }
}
