using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public  class PlayerInputHandler : MonoBehaviour
{
    public InputAction MoveAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction ShootAction { get; private set; }
    public InputAction AimAction { get; private set; }

    private PlayerInput playerInput;


    public static PlayerInputHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one PlayerInputHandler, the old one destroyed");
        }
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        MoveAction = playerInput.actions["Move"];
        JumpAction = playerInput.actions["Jump"];
        ShootAction = playerInput.actions["Shoot"];
        AimAction = playerInput.actions["Aim"];
    }
}
