using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTransition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance = 2f;

    private Transform cameraTransform;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Animator animator;



    private InputAction shootAction;


    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;


    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }


    //private void OnEnable()
    //{
    //    shootAction = PlayerInputHandler.Instance.ShootAction;
    //    shootAction.performed += _ => ShootBullet();
    //}


    //private void OnDisable()
    //{
    //    shootAction.performed -= _ => ShootBullet();
    //}

    //private void ShootBullet()
    //{
    //    RaycastHit hit;
    //    var bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity, this.transform);
    //    var projectile = bullet.GetComponent<Projectile>();
    //    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
    //    {
    //        projectile.Target = hit.point;
    //        projectile.Hit = true;
    //    }
    //    else
    //    {
    //        projectile.Target = cameraTransform.position + cameraTransform.position * 25;
    //        projectile.Hit = false;
    //    }
         
    //}

    void Update()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance; 
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        var moveInput = PlayerInputHandler.Instance.MoveAction.ReadValue<Vector2>();

        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, moveInput, ref animationVelocity, animationSmoothTime);
        var move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat("moveX", currentAnimationBlendVector.x);
        animator.SetFloat("moveZ", currentAnimationBlendVector.y);

        // Changes the height position of the player..

        var jumpInput = PlayerInputHandler.Instance.JumpAction.triggered;
        if (jumpInput && groundedPlayer)
        {
            //animator.SetBool("jump", true);
            animator.CrossFade("jump", animationPlayTransition);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        var cameraAngleY = cameraTransform.eulerAngles.y;
        var rotation = Quaternion.Euler(0,cameraAngleY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
