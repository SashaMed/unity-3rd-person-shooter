using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;
    [SerializeField] private bool autoReload = true;
    [SerializeField] private int reloadFillIterations = 10;
    [SerializeField] private int firstActiveGunIndex = 0;

    private PlayerController playerController;
    public GunSystem ActiveGun { get; private set; }

    private int currentWeaponIndex = 0;
    private RigBuilder rigBuilder;
    private float deltaForReloadVolume;
    private GunSystem activeGun;
    private bool isReloading;


    private void Start()
    {
        playerController= GetComponent<PlayerController>();
        rigBuilder = GetComponent<RigBuilder>();
        if (firstActiveGunIndex >= guns.Length)
        {
            firstActiveGunIndex = 0;
        }
        var gun = guns[firstActiveGunIndex].GetComponent<GunSystem>();

        SetActiveWeapon(gun);
        for (int i = 0; i < guns.Length; i++)
        {
            if (i == firstActiveGunIndex) continue;
            var g = guns[i].GetComponent<GunSystem>();
            g.SetActiveGunModel(false);
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (playerController.IsDead)
        {
            return;
        }
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
        CheckForWeaponChange();

    }

    private void CheckForWeaponChange()
    {

        float z = PlayerInputHandler.Instance.ScrollAction.ReadValue<float>();
        int dir = 0;
        if (z > 0)
        {
            dir = 1;
        }
        else if (z < 0) 
        {
            dir = -1;
        }
        ChangeActiveWeapon(dir);
    }

    private void ChangeActiveWeapon(int dir)
    {
        if (guns.Length < 2)
        {
            return;
        }
        DisableCurrentActiveWeapon();
        currentWeaponIndex += dir;
        if (dir == 1)
        {
            currentWeaponIndex = (currentWeaponIndex % guns.Length == 0) ? 0 : currentWeaponIndex;
        }
        else
        {
            currentWeaponIndex = (currentWeaponIndex < 0) ? guns.Length -1 : currentWeaponIndex;
        }
        var gun = guns[currentWeaponIndex].GetComponent<GunSystem>();
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


    public bool AddAmmo(int amount, GunType ammoType)
    {
        bool res = false;
        for(int i = 0; i < guns.Length;i ++)
        {
            var gunsSripts = guns[i].GetComponent<GunSystem>();
            if (gunsSripts.type == ammoType)
            {
                res = gunsSripts.AddAmmo(amount, ammoType);
                break;
            }
        }

        return res;
    }
}
