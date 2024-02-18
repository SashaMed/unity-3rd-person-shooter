using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerRunMultiplier = 1.4f;
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

    private bool isDead = false;

    private InputAction shootAction;


    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;


    private void Start()
    {
        var health = GetComponent<PlayerHealth>();
        health.OnDeath += Death;
        animator = GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Death(Vector3 pos)
    {
        isDead= true;
        animator.CrossFade("death", animationPlayTransition);
    }

    void Update()
    {
        if (isDead) 
        {
            return;
        }
        HorizontalMovement();
        SetAnimationParameters();
        VerticalMovement();
        PlayerRotation();
    }


    private void PlayerRotation()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
        var cameraAngleY = cameraTransform.eulerAngles.y;
        var rotation = Quaternion.Euler(0, cameraAngleY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void VerticalMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        var jumpInput = PlayerInputHandler.Instance.JumpAction.triggered;
        if (jumpInput && groundedPlayer)
        {
            animator.CrossFade("jump", animationPlayTransition);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    private void HorizontalMovement()
    {
        var moveInput = PlayerInputHandler.Instance.MoveAction.ReadValue<Vector2>();

        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, moveInput, ref animationVelocity, animationSmoothTime);
        var move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        if (Keyboard.current.shiftKey.isPressed)
        {
            move *= playerRunMultiplier;
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
    }

    private void SetAnimationParameters()
    {
        animator.SetFloat("moveX", currentAnimationBlendVector.x);
        animator.SetFloat("moveZ", currentAnimationBlendVector.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        var collectible = other.GetComponent<ICollectible>();
        if (collectible != null)
        {
            collectible.Collect(gameObject);
        }
    }
}
