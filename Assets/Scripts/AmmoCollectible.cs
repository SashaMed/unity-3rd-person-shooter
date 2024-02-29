using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour, ICollectible
{

    [SerializeField] private GunType gunType;
    [SerializeField] private int ammoAmount = 60;
    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject takeParticle;
    [SerializeField] private AudioClip takeSound;

    public void Collect(GameObject player)
    {
        var weaponController = player.GetComponent<WeaponController>();
        if (weaponController != null)
        {
            var result = weaponController.AddAmmo(ammoAmount, gunType);
            if (result)
            {
                gameObject.SetActive(false);
                particle.SetActive(false);
                SoundPool.SoundInstance.PlayVFXSound(takeSound, transform.position); 
                Instantiate(takeParticle,transform.position, Quaternion.identity);
            }
        }
    }
}
