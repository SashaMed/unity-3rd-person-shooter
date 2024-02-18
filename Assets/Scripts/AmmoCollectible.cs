using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour, ICollectible
{

    [SerializeField] private GunType gunType;
    [SerializeField] private int ammoAmount = 60;
    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject takeParticle;


    public void Collect(GameObject player)
    {
        var weaponController = player.GetComponent<WeaponController>();
        if (weaponController != null)
        {
            var result = weaponController.ActiveGun.AddAmmo(ammoAmount, gunType);
            if (result)
            {
                gameObject.SetActive(false);
                particle.SetActive(false);
                Instantiate(takeParticle,transform.position, Quaternion.identity);
            }
        }
    }
}
