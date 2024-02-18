using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Config", order = 3)]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int maxAmmo = 120;
    public int clipSize = 30;

    public int currentAmmo = 120;
    public int currentClipAmmo = 30;

    public float reloadTime = 1.5f;

    public void Reload()
    {
        int maxReloadAmount = Mathf.Min(clipSize, currentAmmo);
        int availableBulletsInCurrentClip = clipSize - currentClipAmmo;
        int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
        currentClipAmmo += reloadAmount;
        currentAmmo -= reloadAmount;
    }

    public bool CanReload()
    {
        return currentClipAmmo < clipSize && currentAmmo > 0;
    }

    public bool AddAmmo(int Amount)
    {
        if (currentAmmo >= maxAmmo) 
        {
            return false;
        }
        if (currentAmmo + Amount > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += Amount;
        }
        return true;
    }


    public bool CanShoot() => currentClipAmmo > 0;
}
