using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] gunsList;

    private GunScript activeGun;

    private void Start()
    {
        activeGun = gunsList[0].GetComponent<GunScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeGun != null)
        {
            activeGun.Tick(Mouse.current.leftButton.isPressed);
        }
    }
}
