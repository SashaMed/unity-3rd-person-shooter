using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] gunsList;
    [SerializeField] private bool autoReload = true;
    [SerializeField] private int reloadFillIterations = 10;

    public GunSystem ActiveGun { get; private set; }

    private int currentWeaponIndex = 0;
    private RigBuilder rigBuilder;
    private float deltaForReloadVolume;
    private GunSystem activeGun;
    private bool isReloading;

    private void Start()
    {
        rigBuilder = GetComponent<RigBuilder>();
        var gun = gunsList[0].GetComponent<GunSystem>();
        SetActiveWeapon(gun);
    }




    // Update is called once per frame
    void Update()
    {
        if (activeGun != null)
        {
            activeGun.Tick(Mouse.current.leftButton.isPressed);
        }

        if (ShouldManualReload() || ShouldAutoReload())
        {
            StartCoroutine(ReloadCoroutine());
        }
        if (!isReloading)
        {
            SetUI();
        }

        if (Mouse.current.middleButton.isPressed)
        {
            ChangeActiveWeapon();
        }

        float z = PlayerInputHandler.Instance.ScrollAction.ReadValue<float>();
        if (z != 0)
        {
            ChangeActiveWeapon();
        }
    }

    private void ChangeActiveWeapon()
    {
        if (gunsList.Length < 2)
        {
            return;
        }
        DisableCurrentActiveWeapon();
        currentWeaponIndex = (++currentWeaponIndex % gunsList.Length == 0) ? 0 : currentWeaponIndex;
        var gun = gunsList[currentWeaponIndex].GetComponent<GunSystem>();
        SetActiveWeapon(gun);
    }

    private void SetActiveWeapon(GunSystem gun)
    {
        rigBuilder.layers[gun.rigBuilderLayerNumber].active = true;
        activeGun = gun;
        ActiveGun = activeGun;
        deltaForReloadVolume = 1f / gun.GetClipSize();
        activeGun.SetActiveGunModel(true);
        LevelUIController.Instance.SetGunName(activeGun.gunName);
    }


    private void DisableCurrentActiveWeapon()
    {
        rigBuilder.layers[activeGun.rigBuilderLayerNumber].active = false;
        activeGun.SetActiveGunModel(false);
    }




    private void SetUI()
    {
        LevelUIController.Instance.SetFillForReloadImage(deltaForReloadVolume * activeGun.GetCurrentClipAmmo());
        LevelUIController.Instance.SetAmmoAmountText(activeGun.GetAmmoAmoubtInfoString());
    }

    private IEnumerator ReloadCoroutine()
    {
        SetUI();
        activeGun.PlayReloadSound();
        isReloading = true;
        var reloadTime = activeGun.GetReloadTime();
        var deltaReloadTime = reloadTime / reloadFillIterations;
        var fillAmount = 0f;
        var deltaFillAmout = 1f / reloadFillIterations;
        LevelUIController.Instance.SetFillForReloadImage(fillAmount);

        for (int i =0; i < reloadFillIterations; i++)
        {
            yield return new WaitForSeconds(deltaReloadTime);
            fillAmount += deltaFillAmout;
            LevelUIController.Instance.SetFillForReloadImage(fillAmount);
        }
        isReloading = false;
        activeGun.Reload();
    }


    private bool ShouldManualReload()
    {
        return !isReloading
            && Keyboard.current.rKey.wasReleasedThisFrame
            && activeGun.CanReload();
    }

    private bool ShouldAutoReload()
    {
        return !isReloading
            && autoReload
            && activeGun.ShouldAutoReload()
            && activeGun.CanReload();
    }
}
