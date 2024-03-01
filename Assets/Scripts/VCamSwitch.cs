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

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = PlayerInputHandler.Instance.AimAction;
        SetEvents();
    }


    private void OnEnable()
    {
        if (aimAction == null)
        {
            return;
        }
        SetEvents();
    }

    private void SetEvents()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        LevelUIController.Instance.SetActiveAimImage(false);
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        LevelUIController.Instance.SetActiveAimImage(true);
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
}
