using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormaInputX { get; set; }
    public int NormaInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool RunInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool[] AttackInput { get; private set; }

    public bool PrimaryAttack { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    [SerializeField] private float dashInputIgnoreTime = 1f;
    private float jumpInputStartTime;
    private float triggerInputStartTime;
    private float dialogInputStartTime;
    private float dashInputStartTime;

    private PlayerInput playerInput;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormaInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormaInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        dashInputStartTime = Time.time;
        RunInput = true;
    }




    public void StopInput()
    {
        NormaInputX = 0;
        NormaInputY = 0;
        RunInput = false;
        JumpInput = false;
        JumpInputStop = true;
    }
}
