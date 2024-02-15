using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject thirdPersonIcon;
    [SerializeField] private GameObject AimIcon;


    public static LevelUIController Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeAimImageToThirdPersonIcon();
    }

    public void ChangeAimImageToThirdPersonIcon()
    {
        thirdPersonIcon.SetActive(true);
        AimIcon.SetActive(false);
    }

    public void ChangeAimImageToAimIicon()
    {
        thirdPersonIcon.SetActive(false);
        AimIcon.SetActive(true);
    }

}
