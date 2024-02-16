using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VCamSwitch : MonoBehaviour
{
    [SerializeField] private int priorityBoostAmount = 10;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = PlayerInputHandler.Instance.AimAction;
    }


    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        LevelUIController.Instance.ChangeAimImageToThirdPersonIcon();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        LevelUIController.Instance.ChangeAimImageToAimIicon();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
}
