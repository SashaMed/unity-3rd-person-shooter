using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; set => wallCheck = value; }
    public Transform LedgeCheckHorizontal { get => ledgeCheckHorizontal; set => ledgeCheckHorizontal = value; }
    public Transform LedgeCheckVertical { get => ledgeCheckVertical; set => ledgeCheckVertical = value; }


    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;

    [SerializeField] private float wallCheckRadius;
    public float groundCheckRadius = 0.2f;

    [SerializeField] private LayerMask whatIsStaticPlatform;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsMovingPlatform;

    public bool CheckIsTouchingWall() => Physics.OverlapSphere(wallCheck.position, wallCheckRadius, whatIsGround).Length != 0;

    public bool CheckIsTouchingWallByBack() => Physics.Raycast(wallCheck.position, Vector2.left * core.Movement.FacingDirection, wallCheckRadius, whatIsGround);

    public bool CheckIsTouchingLedgeHorizontal() => Physics.Raycast(ledgeCheckHorizontal.position, Vector3.forward * core.Movement.FacingDirection,
            wallCheckRadius  * 2, whatIsGround);
    
    public bool CheckIsLedgeAhead() => Physics.Raycast(LedgeCheckVertical.position, Vector3.down, wallCheckRadius, whatIsGround);

    public bool CheckIfGrounded() => Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround).Length != 0;
    public bool CheckIsOnMovingPlatform() => Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsMovingPlatform).Length != 0;
    public bool CheckIsOnStaticPlatform() => Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsStaticPlatform).Length != 0;
}
